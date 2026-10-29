using GameZone.Api.Data;
using GameZone.Api.DTOs;
using GameZone.Api.Exceptions;
using GameZone.Api.Helpers;
using GameZone.Api.Interfaces;
using GameZone.Api.Mappings;
using GameZone.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _db;

    public CategoryService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken ct = default)
    {
        var categories = await _db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(ct);

        return categories.Select(c => c.ToDto()).ToList();
    }

    public async Task<CategoryDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var category = await _db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new NotFoundException($"Category {id} was not found.");

        return category.ToDto();
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default)
    {
        var name = dto.Name.Trim();
        var slug = BuildSlug(name);

        if (await _db.Categories.AnyAsync(c => c.Slug == slug, ct))
            throw new ConflictException($"A category with the slug '{slug}' already exists.");

        var category = new Category
        {
            Name = name,
            Slug = slug,
            Description = dto.Description?.Trim()
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync(ct);

        return category.ToDto();
    }

    public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct = default)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new NotFoundException($"Category {id} was not found.");

        var name = dto.Name.Trim();
        var slug = BuildSlug(name);

        if (await _db.Categories.AnyAsync(c => c.Slug == slug && c.Id != id, ct))
            throw new ConflictException($"A category with the slug '{slug}' already exists.");

        category.Name = name;
        category.Slug = slug;
        category.Description = dto.Description?.Trim();

        await _db.SaveChangesAsync(ct);

        return category.ToDto();
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new NotFoundException($"Category {id} was not found.");

        if (await _db.Products.AnyAsync(p => p.CategoryId == id, ct))
            throw new ConflictException("This category still has products. Move or delete them first.");

        _db.Categories.Remove(category);
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