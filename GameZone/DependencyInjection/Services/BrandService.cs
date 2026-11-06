using Microsoft.EntityFrameworkCore;
using GameZone.Data;
using GameZone.Models;
using GameZone.DTOs.Brand;
using GameZone.DependencyInjection.Interfaces;

namespace GameZone.DependencyInjection.Services
{
    public class BrandService : IBrandService
    {
        private readonly DataContext _context;

        public BrandService(DataContext context)
        {
            _context = context;
        }

        public async Task<BrandResponse> Create(CreateBrandRequest request)
        {
            var brandExists = await _context.Brands
                .AnyAsync(b => b.Name == request.Name || b.Slug == request.Slug);

            if (brandExists)
                throw new Exception("Brand name or slug already exists.");

            var brand = new Brand
            {
                Name = request.Name,
                Slug = request.Slug,
                Description = request.Description,
                LogoUrl = request.LogoUrl
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return MapToResponse(brand);
        }

        public async Task<List<BrandResponse>> GetAll()
        {
            var brands = await _context.Brands
                .Include(b => b.Products)
                .ToListAsync();

            return brands.Select(MapToResponse).ToList();
        }

        public async Task<BrandResponse?> GetById(int id)
        {
            var brand = await _context.Brands
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
                return null;

            return MapToResponse(brand);
        }

        public async Task<bool> Update(int id, UpdateBrandRequest request)
        {
            var brand = await _context.Brands
                .FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
                return false;

            var duplicateExists = await _context.Brands
                .AnyAsync(b =>
                    b.Id != id &&
                    (b.Name == request.Name || b.Slug == request.Slug));

            if (duplicateExists)
                throw new Exception("Brand name or slug already exists.");

            brand.Name = request.Name;
            brand.Slug = request.Slug;
            brand.Description = request.Description;
            brand.LogoUrl = request.LogoUrl;
            brand.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var brand = await _context.Brands
                .FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
                return false;

            if (await _context.Products.AnyAsync(p => p.BrandId == id))
                throw new Exception("Cannot delete a brand that has products.");

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return true;
        }

        private static BrandResponse MapToResponse(Brand brand)
        {
            return new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                Slug = brand.Slug,
                Description = brand.Description,
                LogoUrl = brand.LogoUrl,
                ProductCount = brand.Products.Count,
                CreatedAt = brand.CreatedAt
            };
        }
    }
}