using GameZone.Api.Data;
using GameZone.Api.DTOs;
using GameZone.Api.Interfaces;
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
        return await _db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description
            })
            .ToListAsync(ct);
    }
}