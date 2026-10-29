using GameZone.Api.DTOs;
using GameZone.Api.Models;

namespace GameZone.Api.Mappings;

public static class BrandMappings
{
    public static BrandDto ToDto(this Brand brand) => new()
    {
        Id = brand.Id,
        Name = brand.Name,
        Slug = brand.Slug,
        LogoUrl = brand.LogoUrl
    };
}