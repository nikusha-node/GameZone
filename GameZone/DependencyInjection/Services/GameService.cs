
using GameZone.Data;
using GameZone.DependencyInjection.Interfaces;
using GameZone.DTOs.Common;
using GameZone.DTOs.Game;
using GameZone.Enums;
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

            var genres = await _context.Genres
                .Where(g => request.GenreIds.Contains(g.Id))
                .ToListAsync();

            if (genres.Count != request.GenreIds.Distinct().Count())
                throw new NotFoundException("One or more genres were not found.");

            var game = new Game
            {
                Name = request.Name,
                Description = request.Description,
                ReleaseDate = request.ReleaseDate,
                Developer = request.Developer,
                Publisher = request.Publisher,
                ProductId = request.ProductId,
                Platforms = platforms,
                Genres = genres,
                AgeRating = request.AgeRating
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return MapToResponse(game);
        }



        public async Task<PaginationResponse<GameResponse>> GetAll(
            string? search,
            string? platform,
            string? genre,
            decimal? minPrice,
            decimal? maxPrice,
            bool? onSale,
            AgeRating? ageRating,
            string? sortBy,
            int page,
            int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var query = _context.Games
                .Include(g => g.Platforms)
                .Include(g => g.Genres)
                .Include(g => g.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(g => g.Name.Contains(search));

            if (!string.IsNullOrWhiteSpace(platform))
                query = query.Where(g =>
                    g.Platforms.Any(p => p.Name == platform));

            if (!string.IsNullOrWhiteSpace(genre))
                query = query.Where(g =>
                    g.Genres.Any(gen => gen.Name == genre));

            if (minPrice.HasValue)
                query = query.Where(g => g.Product.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(g => g.Product.Price <= maxPrice.Value);

            if (onSale.HasValue)
                query = query.Where(g => g.Product.IsOnSale == onSale.Value);

            if (ageRating.HasValue)
                query = query.Where(g => g.AgeRating == ageRating.Value);

            query = sortBy?.ToLower() switch
            {
                "price-asc" => query.OrderBy(g => g.Product.Price),
                "price-desc" => query.OrderByDescending(g => g.Product.Price),
                "newest" => query.OrderByDescending(g => g.ReleaseDate),
                "oldest" => query.OrderBy(g => g.ReleaseDate),
                _ => query.OrderBy(g => g.Name)
            };

            var totalCount = await query.CountAsync();

            var games = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResponse<GameResponse>
            {
                Items = games.Select(MapToResponse).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }



        public async Task<GameResponse?> GetById(int id)
        {
            var game = await _context.Games
                .Include(g => g.Platforms)
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(g => g.Id == id);

            return game == null ? null : MapToResponse(game);
        }

        public async Task<bool> Update(int id, UpdateGameRequest request)
        {
            var game = await _context.Games
                .Include(g => g.Platforms)
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
                throw new NotFoundException("Game not found.");

            var platforms = await _context.Platforms
                .Where(p => request.PlatformIds.Contains(p.Id))
                .ToListAsync();

            if (platforms.Count != request.PlatformIds.Distinct().Count())
                throw new NotFoundException("One or more platforms were not found.");

            var genres = await _context.Genres
                .Where(g => request.GenreIds.Contains(g.Id))
                .ToListAsync();

            if (genres.Count != request.GenreIds.Distinct().Count())
                throw new NotFoundException("One or more genres were not found.");

            game.Name = request.Name;
            game.Description = request.Description;
            game.ReleaseDate = request.ReleaseDate;
            game.Developer = request.Developer;
            game.Publisher = request.Publisher;
            game.Platforms = platforms;
            game.Genres = genres;
            game.AgeRating = request.AgeRating;
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
                Genres = game.Genres.Select(g => g.Name).ToList(),
                AgeRating = game.AgeRating,
                ProductId = game.ProductId,
                CreatedAt = game.CreatedAt
            };
        }
    }
}
