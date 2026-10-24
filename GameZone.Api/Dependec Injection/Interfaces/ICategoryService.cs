using GameZone.Api.DTOs;

namespace GameZone.Api.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync(CancellationToken ct = default);
}