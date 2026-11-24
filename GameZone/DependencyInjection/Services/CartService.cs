
using GameZone.Data;
using GameZone.DTOs.Cart;
using GameZone.DependencyInjection.Interfaces;
using GameZone.Exceptions;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.DependencyInjection.Services
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;

        public CartService(DataContext context)
        {
            _context = context;
        }

        public async Task<CartResponse> GetCart(int userId)
        {
            var cart = await GetOrCreateCart(userId);

            return MapToResponse(cart);
        }

        public async Task<CartResponse> AddToCart(
            int userId,
            AddToCartRequest request)
        {
            if (request.Quantity <= 0)
                throw new BadRequestException("Quantity must be greater than zero.");

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId);

            if (product == null)
                throw new NotFoundException("Product not found.");

            if (product.StockQuantity <= 0)
                throw new BadRequestException("Product is out of stock.");

            var cart = await GetOrCreateCart(userId);

            var existingItem = cart.Items
                .FirstOrDefault(i => i.ProductId == request.ProductId);

            var newQuantity = (existingItem?.Quantity ?? 0) + request.Quantity;

            if (newQuantity > product.StockQuantity)
                throw new BadRequestException("Not enough stock available.");

            if (existingItem != null)
            {
                existingItem.Quantity = newQuantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = request.Quantity
                });
            }

            await _context.SaveChangesAsync();

            return MapToResponse(cart);
        }

        public async Task<CartResponse> UpdateCartItem(
            int userId,
            int cartItemId,
            UpdateCartItemRequest request)
        {
            if (request.Quantity <= 0)
                throw new BadRequestException("Quantity must be greater than zero.");

            var cart = await GetOrCreateCart(userId);

            var item = cart.Items
                .FirstOrDefault(i => i.Id == cartItemId);

            if (item == null)
                throw new NotFoundException("Cart item not found.");

            if (request.Quantity > item.Product.StockQuantity)
                throw new BadRequestException("Not enough stock available.");

            item.Quantity = request.Quantity;

            await _context.SaveChangesAsync();

            return MapToResponse(cart);
        }

        public async Task<bool> RemoveCartItem(int userId, int cartItemId)
        {
            var cart = await GetOrCreateCart(userId);

            var item = cart.Items
                .FirstOrDefault(i => i.Id == cartItemId);

            if (item == null)
                throw new NotFoundException("Cart item not found.");

            _context.CartItems.Remove(item);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ClearCart(int userId)
        {
            var cart = await GetOrCreateCart(userId);

            _context.CartItems.RemoveRange(cart.Items);

            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<Cart> GetOrCreateCart(int userId)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == userId);

            if (!userExists)
                throw new NotFoundException("User not found.");

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                _context.Carts.Add(cart);

                await _context.SaveChangesAsync();
            }

            return cart;
        }

        private static CartResponse MapToResponse(Cart cart)
        {
            var response = new CartResponse
            {
                Id = cart.Id,
                Items = cart.Items.Select(item => new CartItemResponse
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    ImageUrl = item.Product.ImageUrl,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    Subtotal = item.Product.Price * item.Quantity
                }).ToList()
            };

            response.TotalPrice = response.Items.Sum(item => item.Subtotal);

            return response;
        }
    }
}
