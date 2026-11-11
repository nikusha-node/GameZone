using GameZone.Data;
using GameZone.DTOs.Game;
using GameZone.DependencyInjection.Interfaces;
using GameZone.Exceptions;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.DependencyInjection.Services
{
    public class GameService : IGameService
    {
        private readonly DataContext _context;

        public GameService(DataContext context)
        {
            _context = context;
        }

        public async Task<GameResponse> Create(CreateGameRequest request)
        {
            var productExists = await _context.Products
                .AnyAsync(p => p.Id == request.ProductId);

            if (!productExists)
                throw new NotFoundException("Product not found.");

            var gameAlreadyExists = await _context.Games
                .AnyAsync(g => g.ProductId == request.ProductId);

            if (gameAlreadyExists)
                throw new ConflictException(
                    "This product is already linked to a game.");

            var platforms = await _context.Platforms
                .Where(p => request.PlatformIds.Contains(p.Id))
                .ToListAsync();

            if (platforms.Count != request.PlatformIds.Distinct().Count())
                throw new NotFoundException("One or more platforms were not found.");

            var game = new Game
            {
                Name = request.Name,
                Description = request.Description,
                ReleaseDate = request.ReleaseDate,
                Developer = request.Developer,
                Publisher = request.Publisher,
                ProductId = request.ProductId,
                Platforms = platforms
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return MapToResponse(game);
        }

        public async Task<List<GameResponse>> GetAll()
        {
            var games = await _context.Games
                .Include(g => g.Platforms)
                .OrderBy(g => g.Name)
                .ToListAsync();

            return games.Select(MapToResponse).ToList();
        }

        public async Task<GameResponse?> GetById(int id)
        {
            var game = await _context.Games
                .Include(g => g.Platforms)
                .FirstOrDefaultAsync(g => g.Id == id);

            return game == null ? null : MapToResponse(game);
        }

        public async Task<bool> Update(int id, UpdateGameRequest request)
        {
            var game = await _context.Games
                .Include(g => g.Platforms)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
                throw new NotFoundException("Game not found.");

            var platforms = await _context.Platforms
                .Where(p => request.PlatformIds.Contains(p.Id))
                .ToListAsync();

            if (platforms.Count != request.PlatformIds.Distinct().Count())
                throw new NotFoundException("One or more platforms were not found.");

            game.Name = request.Name;
            game.Description = request.Description;
            game.ReleaseDate = request.ReleaseDate;
            game.Developer = request.Developer;
            game.Publisher = request.Publisher;
            game.Platforms = platforms;
            game.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var game = await _context.Games
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
                throw new NotFoundException("Game not found.");

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return true;
        }

        private static GameResponse MapToResponse(Game game)
        {
            return new GameResponse
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                ReleaseDate = game.ReleaseDate,
                Developer = game.Developer,
                Publisher = game.Publisher,
                Platforms = game.Platforms.Select(p => p.Name).ToList(),
                ProductId = game.ProductId,
                CreatedAt = game.CreatedAt
            };
        }
    }
}
