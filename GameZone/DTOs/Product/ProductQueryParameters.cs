using GameZone.Enums;

namespace GameZone.DTOs.Product;

public class ProductQueryParameters
{
    public string? Search { get; set; }
    public int? BrandId { get; set; }
    public Category? Category { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string SortBy { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}