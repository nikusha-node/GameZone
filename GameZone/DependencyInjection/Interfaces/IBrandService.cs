using GameZone.DTOs.Brand;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IBrandService
    {
        Task<BrandResponse> Create(CreateBrandRequest request);

        Task<List<BrandResponse>> GetAll();

        Task<BrandResponse?> GetById(int id);

        Task<bool> Update(int id, UpdateBrandRequest request);

        Task<bool> Delete(int id);
    }
}