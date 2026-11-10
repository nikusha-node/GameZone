using GameZone.DTOs.Cart;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface ICartService
    {
        Task<CartResponse> GetCart(int userId);

        Task<CartResponse> AddToCart(int userId, AddToCartRequest request);

        Task<CartResponse> UpdateCartItem(int userId, int cartItemId, UpdateCartItemRequest request);

        Task<bool> RemoveCartItem(int userId, int cartItemId);

        Task<bool> ClearCart(int userId);
    }
}