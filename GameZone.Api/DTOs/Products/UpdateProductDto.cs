using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.DTOs;
public class UpdateProductDto
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
    public int CategoryId { get; set; }

    public int? BrandId { get; set; }
}