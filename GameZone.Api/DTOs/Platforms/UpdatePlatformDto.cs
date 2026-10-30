using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.DTOs;

public class UpdatePlatformDto
{
    [Required, StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}