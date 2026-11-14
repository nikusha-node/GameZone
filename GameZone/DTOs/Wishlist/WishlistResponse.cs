namespace GameZone.DTOs.Wishlist
{
    public class WishlistResponse
    {
        public int Id { get; set; }
        public List<WishlistItemResponse> Items { get; set; } = new();
        public int TotalItems { get; set; }
    }
}