namespace GameZone.Api.Models;

public class Cart
{
    public int Id { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public List<CartItem> Items { get; set; } = new List<CartItem>();
}