namespace GameZone.Models
{
    public class Wishlist : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<WishlistItem> Items { get; set; } = new List<WishlistItem>();
    }
}