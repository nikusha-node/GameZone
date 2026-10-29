using GameZone.Api.DTOs;
using GameZone.Api.Helpers;

namespace GameZone.Api.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductListDto>> GetPagedAsync(ProductQueryParams query, CancellationToken ct = default);
    Task<ProductResponseDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ProductResponseDto> GetBySlugAsync(string slug, CancellationToken ct = default);
}