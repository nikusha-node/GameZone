using GameZone.Enums;

namespace GameZone.DTOs.Game
{
    public class UpdateGameRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string Developer { get; set; }
        public required string Publisher { get; set; }
        public List<int> PlatformIds { get; set; } = new();
        public List<int> GenreIds { get; set; } = new();
        public AgeRating AgeRating { get; set; }
    }
}
