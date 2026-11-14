using GameZone.DTOs.Order;
using GameZone.Enums;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IOrderService
    {
        Task<CheckoutResponse> Checkout(int userId);
        Task<OrderListResponse> GetUserOrders(int userId,OrderQueryParameters parameters);
        Task<OrderResponse?> GetOrderById(int userId, int orderId);
        Task<OrderResponse?> GetOrderById(int orderId);
        Task<OrderListResponse> GetAllOrders(OrderQueryParameters parameters);
        Task<bool> UpdateOrderStatus(int orderId, OrderStatus status);
    }
}