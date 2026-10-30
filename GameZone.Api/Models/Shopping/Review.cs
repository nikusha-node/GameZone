namespace GameZone.Api.Models;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;
}