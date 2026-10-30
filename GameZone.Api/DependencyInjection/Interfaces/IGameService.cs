using GameZone.Api.DTOs;
using GameZone.Api.Helpers;

namespace GameZone.Api.Interfaces;

public interface IGameService
{
    // Public
    Task<PagedResult<GameListDto>> GetPagedAsync(GameQueryParams query, CancellationToken ct = default);
    Task<GameResponseDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<GameResponseDto> GetBySlugAsync(string slug, CancellationToken ct = default);

    // Admin
    Task<PagedResult<GameListDto>> GetPagedForAdminAsync(GameQueryParams query, CancellationToken ct = default);
    Task<GameResponseDto> CreateAsync(CreateGameDto dto, CancellationToken ct = default);
    Task<GameResponseDto> UpdateAsync(int id, UpdateGameDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}