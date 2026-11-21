using GameZone.Enums;

namespace GameZone.DTOs.Game;

public class GameResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public required string Developer { get; set; }
    public required string Publisher { get; set; }
    public List<string> Platforms { get; set; } = new();
    public List<string> Genres { get; set; } = new();
    public AgeRating AgeRating { get; set; }
    public int ProductId { get; set; }
    public DateTime CreatedAt { get; set; }
}
