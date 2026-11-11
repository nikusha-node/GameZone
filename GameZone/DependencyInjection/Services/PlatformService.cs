using GameZone.Data;
using GameZone.DTOs.Platform;
using GameZone.DependencyInjection.Interfaces;
using GameZone.Exceptions;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.DependencyInjection.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly DataContext _context;

        public PlatformService(DataContext context)
        {
            _context = context;
        }

        public async Task<PlatformResponse> Create(CreatePlatformRequest request)
        {
            var nameExists = await _context.Platforms
                .AnyAsync(p => p.Name.ToLower() == request.Name.ToLower());

            if (nameExists)
                throw new ConflictException("A platform with this name already exists.");

            var platform = new Platform
            {
                Name = request.Name,
                Manufacturer = request.Manufacturer
            };

            _context.Platforms.Add(platform);
            await _context.SaveChangesAsync();

            return MapToResponse(platform);
        }

        public async Task<List<PlatformResponse>> GetAll()
        {
            var platforms = await _context.Platforms
                .Include(p => p.Games)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return platforms.Select(MapToResponse).ToList();
        }

        public async Task<PlatformResponse?> GetById(int id)
        {
            var platform = await _context.Platforms
                .Include(p => p.Games)
                .FirstOrDefaultAsync(p => p.Id == id);

            return platform == null ? null : MapToResponse(platform);
        }

        public async Task<bool> Update(int id, UpdatePlatformRequest request)
        {
            var platform = await _context.Platforms
                .FirstOrDefaultAsync(p => p.Id == id);

            if (platform == null)
                throw new NotFoundException("Platform not found.");

            var nameExists = await _context.Platforms
                .AnyAsync(p => p.Id != id &&
                               p.Name.ToLower() == request.Name.ToLower());

            if (nameExists)
                throw new ConflictException("A platform with this name already exists.");

            platform.Name = request.Name;
            platform.Manufacturer = request.Manufacturer;
            platform.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var platform = await _context.Platforms
                .Include(p => p.Games)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (platform == null)
                throw new NotFoundException("Platform not found.");

            if (platform.Games.Any())
                throw new ConflictException(
                    "Cannot delete a platform that has games assigned to it.");

            _context.Platforms.Remove(platform);
            await _context.SaveChangesAsync();

            return true;
        }

        private static PlatformResponse MapToResponse(Platform platform)
        {
            return new PlatformResponse
            {
                Id = platform.Id,
                Name = platform.Name,
                Manufacturer = platform.Manufacturer,
                GameCount = platform.Games.Count,
                CreatedAt = platform.CreatedAt
            };
        }
    }
}
