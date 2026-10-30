using GameZone.Api.Data;
using GameZone.Api.DTOs;
using GameZone.Api.Exceptions;
using GameZone.Api.Helpers;
using GameZone.Api.Interfaces;
using GameZone.Api.Mappings;
using GameZone.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Api.Services;

public class PlatformService : IPlatformService
{
    private readonly ApplicationDbContext _db;

    public PlatformService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<PlatformDto>> GetAllAsync(CancellationToken ct = default)
    {
        var platforms = await _db.Platforms.AsNoTracking().OrderBy(p => p.Name).ToListAsync(ct);
        return platforms.Select(p => p.ToDto()).ToList();
    }

    public async Task<PlatformDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var platform = await _db.Platforms.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException($"Platform {id} was not found.");
        return platform.ToDto();
    }

    public async Task<PlatformDto> CreateAsync(CreatePlatformDto dto, CancellationToken ct = default)
    {
        var name = dto.Name.Trim();
        var slug = BuildSlug(name);

        if (await _db.Platforms.AnyAsync(p => p.Slug == slug, ct))
            throw new ConflictException($"A platform with the slug '{slug}' already exists.");

        var platform = new Platform { Name = name, Slug = slug };
        _db.Platforms.Add(platform);
        await _db.SaveChangesAsync(ct);
        return platform.ToDto();
    }

    public async Task<PlatformDto> UpdateAsync(int id, UpdatePlatformDto dto, CancellationToken ct = default)
    {
        var platform = await _db.Platforms.FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException($"Platform {id} was not found.");

        var name = dto.Name.Trim();
        var slug = BuildSlug(name);

        if (await _db.Platforms.AnyAsync(p => p.Slug == slug && p.Id != id, ct))
            throw new ConflictException($"A platform with the slug '{slug}' already exists.");

        platform.Name = name;
        platform.Slug = slug;
        await _db.SaveChangesAsync(ct);
        return platform.ToDto();
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var platform = await _db.Platforms.FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException($"Platform {id} was not found.");

        if (await _db.Games.AnyAsync(g => g.PlatformId == id, ct))
            throw new ConflictException("This platform still has games. Move or delete them first.");

        _db.Platforms.Remove(platform);
        await _db.SaveChangesAsync(ct);
    }

    private static string BuildSlug(string name)
    {
        var slug = SlugHelper.Generate(name);
        if (string.IsNullOrEmpty(slug))
            throw new BadRequestException("The name must contain at least one letter or number (a-z, 0-9).");
        return slug;
    }
}