using GameZone.DTOs.Genre;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IGenreService
    {
        Task<GenreResponse> Create(CreateGenreRequest request);
        Task<List<GenreResponse>> GetAll();
        Task<GenreResponse?> GetById(int id);
        Task<bool> Update(int id, UpdateGenreRequest request);
        Task<bool> Delete(int id);
    }
}
