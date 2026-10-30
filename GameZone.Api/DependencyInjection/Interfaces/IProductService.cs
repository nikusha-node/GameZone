using GameZone.Api.DTOs;
using GameZone.Api.Helpers;

namespace GameZone.Api.Interfaces;

public interface IProductService
{
    // Public
    Task<PagedResult<ProductListDto>> GetPagedAsync(ProductQueryParams query, CancellationToken ct = default);
    Task<ProductResponseDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ProductResponseDto> GetBySlugAsync(string slug, CancellationToken ct = default);

    // Admin
    Task<PagedResult<ProductListDto>> GetPagedForAdminAsync(ProductQueryParams query, CancellationToken ct = default);
    Task<ProductResponseDto> CreateAsync(CreateProductDto dto, CancellationToken ct = default);
    Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto, CancellationToken ct = default);
    Task UpdateStockAsync(int id, UpdateStockDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<ProductResponseDto> AddImageAsync(int id, ProductImageInputDto dto, CancellationToken ct = default);
    Task RemoveImageAsync(int id, int imageId, CancellationToken ct = default);
    Task<ProductResponseDto> AddSpecificationAsync(int id, ProductSpecificationInputDto dto, CancellationToken ct = default);
    Task RemoveSpecificationAsync(int id, int specId, CancellationToken ct = default);
}