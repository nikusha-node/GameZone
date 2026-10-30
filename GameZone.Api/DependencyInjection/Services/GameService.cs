using GameZone.Api.Data;
using GameZone.Api.DTOs;
using GameZone.Api.Exceptions;
using GameZone.Api.Helpers;
using GameZone.Api.Interfaces;
using GameZone.Api.Mappings;
using GameZone.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Api.Services;

public class GameService : IGameService
{
    private readonly ApplicationDbContext _db;

    public GameService(ApplicationDbContext db)
    {
        _db = db;
    }

    // ---------------- Public reads ----------------

    public Task<PagedResult<GameListDto>> GetPagedAsync(GameQueryParams query, CancellationToken ct = default)
        => BuildPagedAsync(query, activeOnly: true, ct);

    public async Task<GameResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var game = await LoadDetails().FirstOrDefaultAsync(g => g.Id == id && g.Product.IsActive, ct)
            ?? throw new NotFoundException($"Game {id} was not found.");
        return await ToResponseAsync(game, ct);
    }

    public async Task<GameResponseDto> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var key = slug.Trim().ToLowerInvariant();
        var game = await LoadDetails().FirstOrDefaultAsync(g => g.Product.Slug == key && g.Product.IsActive, ct)
            ?? throw new NotFoundException($"Game '{slug}' was not found.");
        return await ToResponseAsync(game, ct);
    }

    // ---------------- Admin ----------------

    public Task<PagedResult<GameListDto>> GetPagedForAdminAsync(GameQueryParams query, CancellationToken ct = default)
        => BuildPagedAsync(query, activeOnly: false, ct);

    public async Task<GameResponseDto> CreateAsync(CreateGameDto dto, CancellationToken ct = default)
    {
        ValidatePrices(dto.Price, dto.DiscountPrice);
        await EnsurePlatformExistsAsync(dto.PlatformId, ct);

        var name = dto.Name.Trim();
        var sku = dto.Sku.Trim().ToUpperInvariant();
        var slug = BuildSlug(name);

        if (await _db.Products.AnyAsync(p => p.Slug == slug, ct))
            throw new ConflictException($"A product with the slug '{slug}' already exists.");
        if (await _db.Products.AnyAsync(p => p.Sku == sku, ct))
            throw new ConflictException($"A product with SKU '{sku}' already exists.");

        var product = new Product
        {
            Name = name,
            Slug = slug,
            Sku = sku,
            Description = dto.Description?.Trim(),
            Price = dto.Price,
            DiscountPrice = dto.DiscountPrice,
            StockQuantity = dto.StockQuantity,
            IsActive = dto.IsActive,
            CategoryId = await GetGamesCategoryIdAsync(ct),
            Game = new Game
            {
                PlatformId = dto.PlatformId,
                Genre = dto.Genre?.Trim(),
                Developer = dto.Developer?.Trim(),
                Publisher = dto.Publisher?.Trim(),
                ReleaseDate = dto.ReleaseDate,
                AgeRating = dto.AgeRating?.Trim(),
                IsDigital = dto.IsDigital
            }
        };

        var images = dto.Images.ToList();
        var primaryIndex = images.FindIndex(i => i.IsPrimary);
        if (primaryIndex < 0 && images.Count > 0) primaryIndex = 0;
        for (var i = 0; i < images.Count; i++)
        {
            product.Images.Add(new ProductImage
            {
                Url = images[i].Url.Trim(),
                AltText = images[i].AltText?.Trim(),
                IsPrimary = i == primaryIndex,
                SortOrder = i
            });
        }

        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);

        return await LoadForAdminAsync(product.Game.Id, ct);
    }

    public async Task<GameResponseDto> UpdateAsync(int id, UpdateGameDto dto, CancellationToken ct = default)
    {
        var game = await _db.Games.Include(g => g.Product).FirstOrDefaultAsync(g => g.Id == id, ct)
            ?? throw new NotFoundException($"Game {id} was not found.");

        ValidatePrices(dto.Price, dto.DiscountPrice);
        await EnsurePlatformExistsAsync(dto.PlatformId, ct);

        var name = dto.Name.Trim();
        var sku = dto.Sku.Trim().ToUpperInvariant();
        var slug = BuildSlug(name);
        var productId = game.ProductId;

        if (await _db.Products.AnyAsync(p => p.Slug == slug && p.Id != productId, ct))
            throw new ConflictException($"A product with the slug '{slug}' already exists.");
        if (await _db.Products.AnyAsync(p => p.Sku == sku && p.Id != productId, ct))
            throw new ConflictException($"A product with SKU '{sku}' already exists.");

        var product = game.Product;
        product.Name = name;
        product.Slug = slug;
        product.Sku = sku;
        product.Description = dto.Description?.Trim();
        product.Price = dto.Price;
        product.DiscountPrice = dto.DiscountPrice;
        product.StockQuantity = dto.StockQuantity;
        product.IsActive = dto.IsActive;
        product.UpdatedAtUtc = DateTime.UtcNow;

        game.PlatformId = dto.PlatformId;
        game.Genre = dto.Genre?.Trim();
        game.Developer = dto.Developer?.Trim();
        game.Publisher = dto.Publisher?.Trim();
        game.ReleaseDate = dto.ReleaseDate;
        game.AgeRating = dto.AgeRating?.Trim();
        game.IsDigital = dto.IsDigital;

        await _db.SaveChangesAsync(ct);
        return await LoadForAdminAsync(id, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var game = await _db.Games.Include(g => g.Product).FirstOrDefaultAsync(g => g.Id == id, ct)
            ?? throw new NotFoundException($"Game {id} was not found.");

        game.Product.IsActive = false;
        game.Product.UpdatedAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
    }

    // ---------------- Helpers ----------------

    private async Task<PagedResult<GameListDto>> BuildPagedAsync(GameQueryParams query, bool activeOnly, CancellationToken ct)
    {
        if (query.MinPrice.HasValue && query.MaxPrice.HasValue && query.MinPrice > query.MaxPrice)
            throw new BadRequestException("MinPrice cannot be greater than MaxPrice.");

        var q = _db.Games.AsNoTracking().AsQueryable();
        if (activeOnly) q = q.Where(g => g.Product.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim();
            q = q.Where(g => g.Product.Name.Contains(s)
                          || (g.Genre != null && g.Genre.Contains(s))
                          || (g.Developer != null && g.Developer.Contains(s))
                          || (g.Publisher != null && g.Publisher.Contains(s)));
        }

        if (query.PlatformId.HasValue)
            q = q.Where(g => g.PlatformId == query.PlatformId);

        if (!string.IsNullOrWhiteSpace(query.PlatformSlug))
        {
            var slug = query.PlatformSlug.Trim().ToLowerInvariant();
            q = q.Where(g => g.Platform.Slug == slug);
        }

        if (!string.IsNullOrWhiteSpace(query.Genre))
        {
            var genre = query.Genre.Trim();
            q = q.Where(g => g.Genre == genre);
        }

        if (query.IsDigital.HasValue)
            q = q.Where(g => g.IsDigital == query.IsDigital);

        if (query.MinPrice.HasValue)
            q = q.Where(g => (g.Product.DiscountPrice ?? g.Product.Price) >= query.MinPrice);

        if (query.MaxPrice.HasValue)
            q = q.Where(g => (g.Product.DiscountPrice ?? g.Product.Price) <= query.MaxPrice);

        if (query.InStock == true)
            q = q.Where(g => g.Product.StockQuantity > 0);
        else if (query.InStock == false)
            q = q.Where(g => g.Product.StockQuantity == 0);

        q = (query.Sort?.Trim().ToLowerInvariant()) switch
        {
            "name" => q.OrderBy(g => g.Product.Name).ThenBy(g => g.Id),
            "price_asc" => q.OrderBy(g => g.Product.DiscountPrice ?? g.Product.Price).ThenBy(g => g.Id),
            "price_desc" => q.OrderByDescending(g => g.Product.DiscountPrice ?? g.Product.Price).ThenBy(g => g.Id),
            "release" => q.OrderByDescending(g => g.ReleaseDate).ThenBy(g => g.Id),
            _ => q.OrderByDescending(g => g.Product.CreatedAtUtc).ThenBy(g => g.Id)
        };

        return await q.Select(GameMappings.ListProjection)
                      .ToPagedResultAsync(query.Page, query.PageSize, ct);
    }

    private static void ValidatePrices(decimal price, decimal? discountPrice)
    {
        if (discountPrice.HasValue && discountPrice.Value >= price)
            throw new BadRequestException("DiscountPrice must be lower than Price.");
    }

    private async Task EnsurePlatformExistsAsync(int platformId, CancellationToken ct)
    {
        if (!await _db.Platforms.AnyAsync(p => p.Id == platformId, ct))
            throw new BadRequestException($"Platform {platformId} does not exist.");
    }
    private async Task<int> GetGamesCategoryIdAsync(CancellationToken ct)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Slug == "games", ct);
        if (category is null)
        {
            category = new Category { Name = "Games", Slug = "games", Description = "PC and console games" };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync(ct);
        }
        return category.Id;
    }

    private static string BuildSlug(string name)
    {
        var slug = SlugHelper.Generate(name);
        if (string.IsNullOrEmpty(slug))
            throw new BadRequestException("The name must contain at least one letter or number (a-z, 0-9).");
        return slug;
    }

    private IQueryable<Game> LoadDetails() => _db.Games
        .AsNoTracking()
        .Include(g => g.Platform)
        .Include(g => g.Product).ThenInclude(p => p.Images);

    private async Task<GameResponseDto> LoadForAdminAsync(int id, CancellationToken ct)
    {
        var game = await LoadDetails().FirstOrDefaultAsync(g => g.Id == id, ct)
            ?? throw new NotFoundException($"Game {id} was not found.");
        return await ToResponseAsync(game, ct);
    }

    private async Task<GameResponseDto> ToResponseAsync(Game game, CancellationToken ct)
    {
        var ratings = await _db.Reviews
            .Where(r => r.ProductId == game.ProductId)
            .Select(r => r.Rating)
            .ToListAsync(ct);

        var average = ratings.Count == 0 ? 0 : Math.Round(ratings.Average(), 1);
        return game.ToDto(average, ratings.Count);
    }
}