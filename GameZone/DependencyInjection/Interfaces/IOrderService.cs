using GameZone.DTOs.Order;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IOrderService
    {
        Task<CheckoutResponse> Checkout(int userId);
        Task<List<OrderResponse>> GetUserOrders(int userId);
        Task<OrderResponse?> GetOrderById(int userId, int orderId);
        Task<List<OrderResponse>> GetAllOrders();
        Task<bool> UpdateOrderStatus(int orderId, GameZone.Enums.OrderStatus status);
        Task<OrderResponse?> GetOrderById(int orderId);
    }
}