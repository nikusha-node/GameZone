using GameZone.Api.Data;
using GameZone.Api.DTOs;
using GameZone.Api.Exceptions;
using GameZone.Api.Helpers;
using GameZone.Api.Interfaces;
using GameZone.Api.Mappings;
using GameZone.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Api.Services;

public class BrandService : IBrandService
{
    private readonly ApplicationDbContext _db;

    public BrandService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<BrandDto>> GetAllAsync(CancellationToken ct = default)
    {
        var brands = await _db.Brands.AsNoTracking().OrderBy(b => b.Name).ToListAsync(ct);
        return brands.Select(b => b.ToDto()).ToList();
    }

    public async Task<BrandDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var brand = await _db.Brands.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id, ct)
            ?? throw new NotFoundException($"Brand {id} was not found.");
        return brand.ToDto();
    }

    public async Task<BrandDto> CreateAsync(CreateBrandDto dto, CancellationToken ct = default)
    {
        var name = dto.Name.Trim();
        var slug = BuildSlug(name);

        if (await _db.Brands.AnyAsync(b => b.Slug == slug, ct))
            throw new ConflictException($"A brand with the slug '{slug}' already exists.");

        var brand = new Brand { Name = name, Slug = slug, LogoUrl = dto.LogoUrl?.Trim() };

        _db.Brands.Add(brand);
        await _db.SaveChangesAsync(ct);
        return brand.ToDto();
    }

    public async Task<BrandDto> UpdateAsync(int id, UpdateBrandDto dto, CancellationToken ct = default)
    {
        var brand = await _db.Brands.FirstOrDefaultAsync(b => b.Id == id, ct)
            ?? throw new NotFoundException($"Brand {id} was not found.");

        var name = dto.Name.Trim();
        var slug = BuildSlug(name);

        if (await _db.Brands.AnyAsync(b => b.Slug == slug && b.Id != id, ct))
            throw new ConflictException($"A brand with the slug '{slug}' already exists.");

        brand.Name = name;
        brand.Slug = slug;
        brand.LogoUrl = dto.LogoUrl?.Trim();

        await _db.SaveChangesAsync(ct);
        return brand.ToDto();
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var brand = await _db.Brands.FirstOrDefaultAsync(b => b.Id == id, ct)
            ?? throw new NotFoundException($"Brand {id} was not found.");

        _db.Brands.Remove(brand);
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