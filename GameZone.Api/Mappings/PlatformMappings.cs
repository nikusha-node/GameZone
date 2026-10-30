using GameZone.Api.DTOs;
using GameZone.Api.Models;

namespace GameZone.Api.Mappings;

public static class PlatformMappings
{
    public static PlatformDto ToDto(this Platform platform) => new()
    {
        Id = platform.Id,
        Name = platform.Name,
        Slug = platform.Slug
    };
}