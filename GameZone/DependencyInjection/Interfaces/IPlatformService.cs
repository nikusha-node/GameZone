using GameZone.DTOs.Platform;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IPlatformService
    {
        Task<PlatformResponse> Create(CreatePlatformRequest request);
        Task<List<PlatformResponse>> GetAll();
        Task<PlatformResponse?> GetById(int id);
        Task<bool> Update(int id, UpdatePlatformRequest request);
        Task<bool> Delete(int id);
    }
}
