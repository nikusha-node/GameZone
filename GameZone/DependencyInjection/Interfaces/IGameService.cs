using GameZone.DTOs.Game;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IGameService
    {
        Task<GameResponse> Create(CreateGameRequest request);
        Task<List<GameResponse>> GetAll();
        Task<GameResponse?> GetById(int id);
        Task<bool> Update(int id, UpdateGameRequest request);
        Task<bool> Delete(int id);
    }
}
