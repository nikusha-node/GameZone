using GameZone.DTOs.Wishlist;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IWishlistService
    {
        Task<WishlistResponse> GetWishlist(int userId);

        Task<WishlistResponse> AddProduct(int userId, int productId);

        Task<bool> RemoveProduct(int userId, int productId);

        Task<bool> ClearWishlist(int userId);
    }
}
