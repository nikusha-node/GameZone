using GameZone.Data;
using GameZone.DependencyInjection.Interfaces;
using GameZone.DTOs.Product;
using GameZone.Exceptions;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;

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
                throw new NotFoundException("Brand not found.");

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

        public async Task<ProductListResponse> GetFiltered(ProductQueryParameters parameters)
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                query = query.Where(p =>
                    p.Name.Contains(parameters.Search));
            }

            if (parameters.BrandId.HasValue)
            {
                query = query.Where(p =>
                    p.BrandId == parameters.BrandId.Value);
            }

            if (parameters.Category.HasValue)
            {
                query = query.Where(p =>
                    p.Category == parameters.Category.Value);
            }

            if (parameters.MinPrice.HasValue)
            {
                query = query.Where(p =>
                    p.Price >= parameters.MinPrice.Value);
            }

            if (parameters.MaxPrice.HasValue)
            {
                query = query.Where(p =>
                    p.Price <= parameters.MaxPrice.Value);
            }

            var totalCount = await query.CountAsync();

            query = parameters.SortBy.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "oldest" => query.OrderBy(p => p.CreatedAt),
                "name_asc" => query.OrderBy(p => p.Name),
                "name_desc" => query.OrderByDescending(p => p.Name),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            var page = Math.Max(1, parameters.Page);
            var pageSize = Math.Clamp(parameters.PageSize, 1, 50);

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ProductListResponse
            {
                Products = products.Select(MapToResponse).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
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
                throw new NotFoundException("Brand not found.");

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