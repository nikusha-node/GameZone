using GameZone.Api.DTOs;

namespace GameZone.Api.Interfaces;

public interface IBrandService
{
    Task<List<BrandDto>> GetAllAsync(CancellationToken ct = default);
    Task<BrandDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<BrandDto> CreateAsync(CreateBrandDto dto, CancellationToken ct = default);
    Task<BrandDto> UpdateAsync(int id, UpdateBrandDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}