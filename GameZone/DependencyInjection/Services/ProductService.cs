using Microsoft.EntityFrameworkCore;
using GameZone.Data;
using GameZone.Models;
using GameZone.DTOs.Product;
using GameZone.DependencyInjection.Interfaces;

namespace GameZone.DependencyInjection.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ProductResponse> Create(CreateProductRequest request)
        {
            var brand = await _context.Brands
                .FirstOrDefaultAsync(b => b.Id == request.BrandId);

            if (brand == null)
                throw new Exception("Brand not found.");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                ImageUrl = request.ImageUrl,
                BrandId = request.BrandId,
                Brand = brand,
                Category = request.Category
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return MapToResponse(product);
        }

        public async Task<List<ProductResponse>> GetAll()
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .ToListAsync();

            return products.Select(MapToResponse).ToList();
        }

        public async Task<ProductResponse?> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return null;

            return MapToResponse(product);
        }

        public async Task<bool> Update(int id, UpdateProductRequest request)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return false;

            var brand = await _context.Brands
                .FirstOrDefaultAsync(b => b.Id == request.BrandId);

            if (brand == null)
                throw new Exception("Brand not found.");

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.ImageUrl = request.ImageUrl;
            product.BrandId = request.BrandId;
            product.Brand = brand;
            product.Category = request.Category;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        private static ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                BrandId = product.BrandId,
                BrandName = product.Brand.Name,
                Category = product.Category,
                CreatedAt = product.CreatedAt
            };
        }
    }
}