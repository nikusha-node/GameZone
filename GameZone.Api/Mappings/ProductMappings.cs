using System.Linq.Expressions;
using GameZone.Api.DTOs;
using GameZone.Api.Models;

namespace GameZone.Api.Mappings;

public static class ProductMappings
{
    public static readonly Expression<Func<Product, ProductListDto>> ListProjection = p => new ProductListDto
    {
        Id = p.Id,
        Name = p.Name,
        Slug = p.Slug,
        Price = p.Price,
        DiscountPrice = p.DiscountPrice,
        FinalPrice = p.DiscountPrice ?? p.Price,
        StockQuantity = p.StockQuantity,
        InStock = p.StockQuantity > 0,
        CategoryName = p.Category.Name,
        BrandName = p.Brand != null ? p.Brand.Name : null,
        PrimaryImageUrl = p.Images
            .OrderByDescending(i => i.IsPrimary).ThenBy(i => i.SortOrder)
            .Select(i => i.Url).FirstOrDefault(),
        AverageRating = p.Reviews.Average(r => (double?)r.Rating) ?? 0,
        ReviewCount = p.Reviews.Count()
    };

    public static ProductResponseDto ToDto(this Product p, double averageRating, int reviewCount) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Slug = p.Slug,
        Sku = p.Sku,
        Description = p.Description,
        Price = p.Price,
        DiscountPrice = p.DiscountPrice,
        FinalPrice = p.DiscountPrice ?? p.Price,
        StockQuantity = p.StockQuantity,
        InStock = p.StockQuantity > 0,
        IsActive = p.IsActive,
        CreatedAtUtc = p.CreatedAtUtc,
        Category = p.Category.ToDto(),
        Brand = p.Brand?.ToDto(),
        Images = p.Images.OrderByDescending(i => i.IsPrimary).ThenBy(i => i.SortOrder)
            .Select(i => new ProductImageDto { Id = i.Id, Url = i.Url, AltText = i.AltText, IsPrimary = i.IsPrimary })
            .ToList(),
        Specifications = p.Specifications.OrderBy(s => s.SortOrder)
            .Select(s => new ProductSpecificationDto { Id = s.Id, Name = s.Name, Value = s.Value })
            .ToList(),
        AverageRating = averageRating,
        ReviewCount = reviewCount
    };
}