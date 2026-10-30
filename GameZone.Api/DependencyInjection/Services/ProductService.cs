using GameZone.Api.Data;
using GameZone.Api.DTOs;
using GameZone.Api.Exceptions;
using GameZone.Api.Helpers;
using GameZone.Api.Interfaces;
using GameZone.Api.Mappings;
using GameZone.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Api.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _db;

    public ProductService(ApplicationDbContext db)
    {
        _db = db;
    }

    // ---------------- Public reads ----------------

    public Task<PagedResult<ProductListDto>> GetPagedAsync(ProductQueryParams query, CancellationToken ct = default)
        => BuildPagedAsync(query, activeOnly: true, ct);

    public async Task<ProductResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var product = await LoadDetails().FirstOrDefaultAsync(p => p.Id == id && p.IsActive, ct)
            ?? throw new NotFoundException($"Product {id} was not found.");
        return await ToResponseAsync(product, ct);
    }

    public async Task<ProductResponseDto> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var key = slug.Trim().ToLowerInvariant();
        var product = await LoadDetails().FirstOrDefaultAsync(p => p.Slug == key && p.IsActive, ct)
            ?? throw new NotFoundException($"Product '{slug}' was not found.");
        return await ToResponseAsync(product, ct);
    }

    // ---------------- Admin ----------------

    public Task<PagedResult<ProductListDto>> GetPagedForAdminAsync(ProductQueryParams query, CancellationToken ct = default)
        => BuildPagedAsync(query, activeOnly: false, ct);

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto, CancellationToken ct = default)
    {
        ValidatePrices(dto.Price, dto.DiscountPrice);
        await EnsureCategoryAndBrandExistAsync(dto.CategoryId, dto.BrandId, ct);

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
            CategoryId = dto.CategoryId,
            BrandId = dto.BrandId
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

        for (var i = 0; i < dto.Specifications.Count; i++)
        {
            product.Specifications.Add(new ProductSpecification
            {
                Name = dto.Specifications[i].Name.Trim(),
                Value = dto.Specifications[i].Value.Trim(),
                SortOrder = i
            });
        }

        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);

        return await LoadForAdminAsync(product.Id, ct);
    }

    public async Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto, CancellationToken ct = default)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException($"Product {id} was not found.");

        ValidatePrices(dto.Price, dto.DiscountPrice);
        await EnsureCategoryAndBrandExistAsync(dto.CategoryId, dto.BrandId, ct);

        var name = dto.Name.Trim();
        var sku = dto.Sku.Trim().ToUpperInvariant();
        var slug = BuildSlug(name);

        if (await _db.Products.AnyAsync(p => p.Slug == slug && p.Id != id, ct))
            throw new ConflictException($"A product with the slug '{slug}' already exists.");
        if (await _db.Products.AnyAsync(p => p.Sku == sku && p.Id != id, ct))
            throw new ConflictException($"A product with SKU '{sku}' already exists.");

        product.Name = name;
        product.Slug = slug;
        product.Sku = sku;
        product.Description = dto.Description?.Trim();
        product.Price = dto.Price;
        product.DiscountPrice = dto.DiscountPrice;
        product.StockQuantity = dto.StockQuantity;
        product.IsActive = dto.IsActive;
        product.CategoryId = dto.CategoryId;
        product.BrandId = dto.BrandId;
        product.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return await LoadForAdminAsync(id, ct);
    }

    public async Task UpdateStockAsync(int id, UpdateStockDto dto, CancellationToken ct = default)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException($"Product {id} was not found.");

        product.StockQuantity = dto.StockQuantity;
        product.UpdatedAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException($"Product {id} was not found.");

        product.IsActive = false;
        product.UpdatedAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
    }

    public async Task<ProductResponseDto> AddImageAsync(int id, ProductImageInputDto dto, CancellationToken ct = default)
    {
        var product = await _db.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException($"Product {id} was not found.");

        var makePrimary = dto.IsPrimary || product.Images.Count == 0;
        if (makePrimary)
            foreach (var existing in product.Images) existing.IsPrimary = false;

        product.Images.Add(new ProductImage
        {
            Url = dto.Url.Trim(),
            AltText = dto.AltText?.Trim(),
            IsPrimary = makePrimary,
            SortOrder = product.Images.Count
        });

        product.UpdatedAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return await LoadForAdminAsync(id, ct);
    }

    public async Task RemoveImageAsync(int id, int imageId, CancellationToken ct = default)
    {
        var image = await _db.ProductImages.FirstOrDefaultAsync(i => i.Id == imageId && i.ProductId == id, ct)
            ?? throw new NotFoundException($"Image {imageId} was not found on product {id}.");

        var wasPrimary = image.IsPrimary;
        _db.ProductImages.Remove(image);
        await _db.SaveChangesAsync(ct);

        if (wasPrimary)
        {
            var next = await _db.ProductImages.Where(i => i.ProductId == id)
                .OrderBy(i => i.SortOrder).FirstOrDefaultAsync(ct);
            if (next is not null)
            {
                next.IsPrimary = true;
                await _db.SaveChangesAsync(ct);
            }
        }
    }

    public async Task<ProductResponseDto> AddSpecificationAsync(int id, ProductSpecificationInputDto dto, CancellationToken ct = default)
    {
        var product = await _db.Products.Include(p => p.Specifications).FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException($"Product {id} was not found.");

        product.Specifications.Add(new ProductSpecification
        {
            Name = dto.Name.Trim(),
            Value = dto.Value.Trim(),
            SortOrder = product.Specifications.Count
        });

        product.UpdatedAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return await LoadForAdminAsync(id, ct);
    }

    public async Task RemoveSpecificationAsync(int id, int specId, CancellationToken ct = default)
    {
        var spec = await _db.ProductSpecifications.FirstOrDefaultAsync(s => s.Id == specId && s.ProductId == id, ct)
            ?? throw new NotFoundException($"Specification {specId} was not found on product {id}.");

        _db.ProductSpecifications.Remove(spec);
        await _db.SaveChangesAsync(ct);
    }

    // ---------------- Helpers ----------------

    private async Task<PagedResult<ProductListDto>> BuildPagedAsync(ProductQueryParams query, bool activeOnly, CancellationToken ct)
    {
        if (query.MinPrice.HasValue && query.MaxPrice.HasValue && query.MinPrice > query.MaxPrice)
            throw new BadRequestException("MinPrice cannot be greater than MaxPrice.");

        var q = _db.Products.AsNoTracking().AsQueryable();
        if (activeOnly) q = q.Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim();
            q = q.Where(p => p.Name.Contains(s)
                          || p.Sku.Contains(s)
                          || (p.Description != null && p.Description.Contains(s)));
        }

        if (query.CategoryId.HasValue)
            q = q.Where(p => p.CategoryId == query.CategoryId);

        if (!string.IsNullOrWhiteSpace(query.CategorySlug))
        {
            var slug = query.CategorySlug.Trim().ToLowerInvariant();
            q = q.Where(p => p.Category.Slug == slug);
        }

        if (query.BrandId.HasValue)
            q = q.Where(p => p.BrandId == query.BrandId);

        if (query.MinPrice.HasValue)
            q = q.Where(p => (p.DiscountPrice ?? p.Price) >= query.MinPrice);

        if (query.MaxPrice.HasValue)
            q = q.Where(p => (p.DiscountPrice ?? p.Price) <= query.MaxPrice);

        if (query.InStock == true)
            q = q.Where(p => p.StockQuantity > 0);
        else if (query.InStock == false)
            q = q.Where(p => p.StockQuantity == 0);

        q = (query.Sort?.Trim().ToLowerInvariant()) switch
        {
            "name" => q.OrderBy(p => p.Name).ThenBy(p => p.Id),
            "price_asc" => q.OrderBy(p => p.DiscountPrice ?? p.Price).ThenBy(p => p.Id),
            "price_desc" => q.OrderByDescending(p => p.DiscountPrice ?? p.Price).ThenBy(p => p.Id),
            _ => q.OrderByDescending(p => p.CreatedAtUtc).ThenBy(p => p.Id)
        };

        return await q.Select(ProductMappings.ListProjection)
                      .ToPagedResultAsync(query.Page, query.PageSize, ct);
    }

    private static void ValidatePrices(decimal price, decimal? discountPrice)
    {
        if (discountPrice.HasValue && discountPrice.Value >= price)
            throw new BadRequestException("DiscountPrice must be lower than Price.");
    }

    private async Task EnsureCategoryAndBrandExistAsync(int categoryId, int? brandId, CancellationToken ct)
    {
        if (!await _db.Categories.AnyAsync(c => c.Id == categoryId, ct))
            throw new BadRequestException($"Category {categoryId} does not exist.");

        if (brandId.HasValue && !await _db.Brands.AnyAsync(b => b.Id == brandId.Value, ct))
            throw new BadRequestException($"Brand {brandId} does not exist.");
    }

    private static string BuildSlug(string name)
    {
        var slug = SlugHelper.Generate(name);
        if (string.IsNullOrEmpty(slug))
            throw new BadRequestException("The name must contain at least one letter or number (a-z, 0-9).");
        return slug;
    }

    private IQueryable<Product> LoadDetails() => _db.Products
        .AsNoTracking()
        .Include(p => p.Category)
        .Include(p => p.Brand)
        .Include(p => p.Images)
        .Include(p => p.Specifications);

    private async Task<ProductResponseDto> LoadForAdminAsync(int id, CancellationToken ct)
    {
        var product = await LoadDetails().FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException($"Product {id} was not found.");
        return await ToResponseAsync(product, ct);
    }

    private async Task<ProductResponseDto> ToResponseAsync(Product product, CancellationToken ct)
    {
        var ratings = await _db.Reviews
            .Where(r => r.ProductId == product.Id)
            .Select(r => r.Rating)
            .ToListAsync(ct);

        var average = ratings.Count == 0 ? 0 : Math.Round(ratings.Average(), 1);
        return product.ToDto(average, ratings.Count);
    }
}