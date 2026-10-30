using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.DTOs;

public class ProductImageInputDto
{
    [Required, Url, StringLength(500)]
    public string Url { get; set; } = string.Empty;

    [StringLength(200)]
    public string? AltText { get; set; }

    public bool IsPrimary { get; set; }
}