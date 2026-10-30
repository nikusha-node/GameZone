using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Models;

public class Review
{
    public int Id { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(100)]
    public string? Title { get; set; }

    [MaxLength(2000)]
    public string? Comment { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
}