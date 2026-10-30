using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.DTOs;

// Images are managed with the product image endpoints (use the game's productId).
public class UpdateGameDto
{
    [Required, StringLength(200, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50, MinimumLength = 2)]
    public string Sku { get; set; } = string.Empty;

    [StringLength(4000)]
    public string? Description { get; set; }

    [Range(0.01, 1000000)]
    public decimal Price { get; set; }

    [Range(0.01, 1000000)]
    public decimal? DiscountPrice { get; set; }

    [Range(0, 1000000)]
    public int StockQuantity { get; set; }

    public bool IsActive { get; set; } = true;

    [Range(1, int.MaxValue)]
    public int PlatformId { get; set; }

    [StringLength(100)]
    public string? Genre { get; set; }

    [StringLength(150)]
    public string? Developer { get; set; }

    [StringLength(150)]
    public string? Publisher { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    [StringLength(20)]
    public string? AgeRating { get; set; }

    public bool IsDigital { get; set; }
}