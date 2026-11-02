using GameZone.DTOs.Product;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> Create(CreateProductRequest request);

        Task<List<ProductResponse>> GetAll();

        Task<ProductResponse?> GetById(int id);

        Task<bool> Update(int id, UpdateProductRequest request);

        Task<bool> Delete(int id);
    }
}