using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Models;

public class ProductSpecification
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(300)]
    public string Value { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}