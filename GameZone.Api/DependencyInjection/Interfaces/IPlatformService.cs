using GameZone.Api.DTOs;

namespace GameZone.Api.Interfaces;

public interface IPlatformService
{
    Task<List<PlatformDto>> GetAllAsync(CancellationToken ct = default);
    Task<PlatformDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PlatformDto> CreateAsync(CreatePlatformDto dto, CancellationToken ct = default);
    Task<PlatformDto> UpdateAsync(int id, UpdatePlatformDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}