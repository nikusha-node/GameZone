using GameZone.Api.DTOs;

namespace GameZone.Api.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync(CancellationToken ct = default);
    Task<CategoryDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default);
    Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}