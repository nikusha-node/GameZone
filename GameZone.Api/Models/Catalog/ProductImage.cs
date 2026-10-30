using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Models;

public class ProductImage
{
    public int Id { get; set; }

    [Required, MaxLength(500)]
    public string Url { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? AltText { get; set; }

    public bool IsPrimary { get; set; }
    public int SortOrder { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}