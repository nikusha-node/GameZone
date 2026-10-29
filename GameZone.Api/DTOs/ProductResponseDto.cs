namespace GameZone.Api.DTOs;

public class ProductImageDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public bool IsPrimary { get; set; }
}

public class ProductSpecificationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool InStock { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public CategoryDto Category { get; set; } = null!;
    public BrandDto? Brand { get; set; }
    public List<ProductImageDto> Images { get; set; } = new();
    public List<ProductSpecificationDto> Specifications { get; set; } = new();
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}