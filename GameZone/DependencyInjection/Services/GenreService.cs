using GameZone.Data;
using GameZone.DTOs.Genre;
using GameZone.DependencyInjection.Interfaces;
using GameZone.Exceptions;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.DependencyInjection.Services
{
    public class GenreService : IGenreService
    {
        private readonly DataContext _context;

        public GenreService(DataContext context)
        {
            _context = context;
        }

        public async Task<GenreResponse> Create(CreateGenreRequest request)
        {
            var nameExists = await _context.Genres
                .AnyAsync(g => g.Name.ToLower() == request.Name.ToLower());

            if (nameExists)
                throw new ConflictException("A genre with this name already exists.");

            var genre = new Genre
            {
                Name = request.Name
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return MapToResponse(genre);
        }

        public async Task<List<GenreResponse>> GetAll()
        {
            var genres = await _context.Genres
                .Include(g => g.Games)
                .OrderBy(g => g.Name)
                .ToListAsync();

            return genres.Select(MapToResponse).ToList();
        }

        public async Task<GenreResponse?> GetById(int id)
        {
            var genre = await _context.Genres
                .Include(g => g.Games)
                .FirstOrDefaultAsync(g => g.Id == id);

            return genre == null ? null : MapToResponse(genre);
        }

        public async Task<bool> Update(int id, UpdateGenreRequest request)
        {
            var genre = await _context.Genres
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
                throw new NotFoundException("Genre not found.");

            var nameExists = await _context.Genres
                .AnyAsync(g => g.Id != id &&
                               g.Name.ToLower() == request.Name.ToLower());

            if (nameExists)
                throw new ConflictException("A genre with this name already exists.");

            genre.Name = request.Name;
            genre.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var genre = await _context.Genres
                .Include(g => g.Games)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
                throw new NotFoundException("Genre not found.");

            if (genre.Games.Any())
                throw new ConflictException(
                    "Cannot delete a genre that has games assigned to it.");

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return true;
        }

        private static GenreResponse MapToResponse(Genre genre)
        {
            return new GenreResponse
            {
                Id = genre.Id,
                Name = genre.Name,
                GameCount = genre.Games.Count,
                CreatedAt = genre.CreatedAt
            };
        }
    }
}
