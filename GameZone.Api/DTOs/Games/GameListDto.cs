namespace GameZone.Api.DTOs;

public class GameListDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public bool InStock { get; set; }
    public string? Genre { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public bool IsDigital { get; set; }
    public string PlatformName { get; set; } = string.Empty;
    public string PlatformSlug { get; set; } = string.Empty;
    public string? PrimaryImageUrl { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}