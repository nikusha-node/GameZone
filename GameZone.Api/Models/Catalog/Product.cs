using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Api.Models;
public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(220)]
    public string Slug { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Sku { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string? Description { get; set; }

    [Precision(18, 2)]
    public decimal Price { get; set; }

    [Precision(18, 2)]
    public decimal? DiscountPrice { get; set; }

    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int? BrandId { get; set; }
    public Brand? Brand { get; set; }

    public Game? Game { get; set; } 

    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}