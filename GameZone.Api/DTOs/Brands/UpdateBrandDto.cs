using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.DTOs;

public class UpdateBrandDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Url, StringLength(500)]
    public string? LogoUrl { get; set; }
}