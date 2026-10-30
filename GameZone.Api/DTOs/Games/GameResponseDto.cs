namespace GameZone.Api.DTOs;

public class GameResponseDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
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
    public string? Genre { get; set; }
    public string? Developer { get; set; }
    public string? Publisher { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public string? AgeRating { get; set; }
    public bool IsDigital { get; set; }
    public PlatformDto Platform { get; set; } = null!;
    public List<ProductImageDto> Images { get; set; } = new();
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}