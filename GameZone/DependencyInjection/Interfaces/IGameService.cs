using GameZone.DTOs.Common;
using GameZone.DTOs.Game;
using GameZone.Enums;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IGameService
    {
        Task<GameResponse> Create(CreateGameRequest request);
        Task<PaginationResponse<GameResponse>> GetAll(
            string? search,
            List<string>? platforms,
            List<string>? genres,
            decimal? minPrice,
            decimal? maxPrice,
            bool? onSale,
            AgeRating? ageRating,
            string? sortBy,
            int page,
            int pageSize
        );
        Task<GameResponse?> GetById(int id);
        Task<bool> Update(int id, UpdateGameRequest request);
        Task<bool> Delete(int id);
    }
}
