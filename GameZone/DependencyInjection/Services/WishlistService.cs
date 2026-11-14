
using GameZone.Data;
using GameZone.DependencyInjection.Interfaces;
using GameZone.DTOs.Wishlist;
using GameZone.Exceptions;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.DependencyInjection.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly DataContext _context;

        public WishlistService(DataContext context)
        {
            _context = context;
        }

        public async Task<WishlistResponse> GetWishlist(int userId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Items)
                    .ThenInclude(wi => wi.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
                throw new NotFoundException("Wishlist not found.");

            return MapToResponse(wishlist);
        }

        public async Task<WishlistResponse> AddProduct(int userId, int productId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new NotFoundException("Product not found.");

            var wishlist = await _context.Wishlists
                .Include(w => w.Items)
                    .ThenInclude(wi => wi.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    UserId = userId
                };

                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            var existingItem = wishlist.Items
                .FirstOrDefault(wi => wi.ProductId == productId);

            if (existingItem != null)
                throw new ConflictException("Product is already in the wishlist.");

            var item = new WishlistItem
            {
                WishlistId = wishlist.Id,
                ProductId = productId,
                Product = product
            };

            wishlist.Items.Add(item);

            await _context.SaveChangesAsync();

            return await GetWishlist(userId);
        }

        public async Task<bool> RemoveProduct(int userId, int productId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
                return false;

            var item = wishlist.Items
                .FirstOrDefault(wi => wi.ProductId == productId);

            if (item == null)
                return false;

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ClearWishlist(int userId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
                return false;

            _context.WishlistItems.RemoveRange(wishlist.Items);
            await _context.SaveChangesAsync();

            return true;
        }

        private static WishlistResponse MapToResponse(Wishlist wishlist)
        {
            return new WishlistResponse
            {
                Id = wishlist.Id,
                Items = wishlist.Items.Select(item => new WishlistItemResponse
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    ImageUrl = item.Product.ImageUrl
                }).ToList(),
                TotalItems = wishlist.Items.Count
            };
        }
    }
}
