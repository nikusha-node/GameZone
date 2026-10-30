
using GameZone.Api.Enum;

namespace GameZone.Api.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsVerified { get; set; } = false;
    public string PhoneNumber { get; set; }
    public Role Role { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string? code { get; set; }

    public Cart Cart { get; set; }
    public List<Order> Orders { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public List<WishlistItem> WishlistItems { get; set; } = new();
}