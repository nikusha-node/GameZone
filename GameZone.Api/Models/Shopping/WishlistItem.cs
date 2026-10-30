namespace GameZone.Api.Models;

public class WishlistItem
{
    public int Id { get; set; }

    public DateTime AddedAtUtc { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}