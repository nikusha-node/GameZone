namespace GameZone.Api.DTOs;

public class ProductListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool InStock { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? BrandName { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}