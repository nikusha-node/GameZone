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

    public async Task<PagedResult<ProductListDto>> GetPagedAsync(ProductQueryParams query, CancellationToken ct = default)
    {
        if (query.MinPrice.HasValue && query.MaxPrice.HasValue && query.MinPrice > query.MaxPrice)
            throw new BadRequestException("MinPrice cannot be greater than MaxPrice.");

        var q = _db.Products.AsNoTracking().Where(p => p.IsActive);

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

    private IQueryable<Product> LoadDetails() => _db.Products
        .AsNoTracking()
        .Include(p => p.Category)
        .Include(p => p.Brand)
        .Include(p => p.Images)
        .Include(p => p.Specifications);

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