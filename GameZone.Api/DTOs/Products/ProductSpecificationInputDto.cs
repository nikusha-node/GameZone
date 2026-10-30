using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.DTOs;

public class ProductSpecificationInputDto
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(300)]
    public string Value { get; set; } = string.Empty;
}