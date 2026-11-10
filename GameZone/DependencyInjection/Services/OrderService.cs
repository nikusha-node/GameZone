
using GameZone.Data;
using GameZone.DTOs.Order;
using GameZone.DependencyInjection.Interfaces;
using GameZone.Enums;
using GameZone.Exceptions;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.DependencyInjection.Services
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;

        public OrderService(DataContext context)
        {
            _context = context;
        }

        public async Task<CheckoutResponse> Checkout(int userId)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
                throw new BadRequestException("Your cart is empty.");

            foreach (var item in cart.Items)
            {
                if (item.Quantity > item.Product.StockQuantity)
                    throw new BadRequestException(
                        $"Not enough stock for {item.Product.Name}.");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                TotalAmount = cart.Items.Sum(
                    item => item.Product.Price * item.Quantity
                )
            };

            foreach (var item in cart.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                });

                item.Product.StockQuantity -= item.Quantity;
            }

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.Items);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new CheckoutResponse
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus.ToString(),
                OrderDate = order.OrderDate
            };
        }

        public async Task<List<OrderResponse>> GetUserOrders(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders.Select(MapToResponse).ToList();
        }

        public async Task<OrderResponse?> GetOrderById(
            int userId, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(
                    o => o.Id == orderId && o.UserId == userId
                );

            return order == null ? null : MapToResponse(order);
        }

        public async Task<List<OrderResponse>> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders.Select(MapToResponse).ToList();
        }

        public async Task<bool> UpdateOrderStatus(
            int orderId, OrderStatus status)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), status))
                throw new BadRequestException("Invalid order status.");

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new NotFoundException("Order not found.");

            order.OrderStatus = status;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        private static OrderResponse MapToResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                Items = order.OrderItems.Select(item =>
                    new OrderItemResponse
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        ImageUrl = item.Product.ImageUrl,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Subtotal = item.UnitPrice * item.Quantity
                    }).ToList()
            };
        }
    }
}
