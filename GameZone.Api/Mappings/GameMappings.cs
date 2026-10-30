using System.Linq.Expressions;
using GameZone.Api.DTOs;
using GameZone.Api.Models;

namespace GameZone.Api.Mappings;

public static class GameMappings
{
    public static readonly Expression<Func<Game, GameListDto>> ListProjection = g => new GameListDto
    {
        Id = g.Id,
        ProductId = g.ProductId,
        Name = g.Product.Name,
        Slug = g.Product.Slug,
        Price = g.Product.Price,
        DiscountPrice = g.Product.DiscountPrice,
        FinalPrice = g.Product.DiscountPrice ?? g.Product.Price,
        InStock = g.Product.StockQuantity > 0,
        Genre = g.Genre,
        ReleaseDate = g.ReleaseDate,
        IsDigital = g.IsDigital,
        PlatformName = g.Platform.Name,
        PlatformSlug = g.Platform.Slug,
        PrimaryImageUrl = g.Product.Images
            .OrderByDescending(i => i.IsPrimary).ThenBy(i => i.SortOrder)
            .Select(i => i.Url).FirstOrDefault(),
        AverageRating = g.Product.Reviews.Average(r => (double?)r.Rating) ?? 0,
        ReviewCount = g.Product.Reviews.Count()
    };

    public static GameResponseDto ToDto(this Game g, double averageRating, int reviewCount) => new()
    {
        Id = g.Id,
        ProductId = g.ProductId,
        Name = g.Product.Name,
        Slug = g.Product.Slug,
        Sku = g.Product.Sku,
        Description = g.Product.Description,
        Price = g.Product.Price,
        DiscountPrice = g.Product.DiscountPrice,
        FinalPrice = g.Product.DiscountPrice ?? g.Product.Price,
        StockQuantity = g.Product.StockQuantity,
        InStock = g.Product.StockQuantity > 0,
        IsActive = g.Product.IsActive,
        Genre = g.Genre,
        Developer = g.Developer,
        Publisher = g.Publisher,
        ReleaseDate = g.ReleaseDate,
        AgeRating = g.AgeRating,
        IsDigital = g.IsDigital,
        Platform = g.Platform.ToDto(),
        Images = g.Product.Images.OrderByDescending(i => i.IsPrimary).ThenBy(i => i.SortOrder)
            .Select(i => new ProductImageDto { Id = i.Id, Url = i.Url, AltText = i.AltText, IsPrimary = i.IsPrimary })
            .ToList(),
        AverageRating = averageRating,
        ReviewCount = reviewCount
    };
}