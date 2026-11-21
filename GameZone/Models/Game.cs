using GameZone.Enums;

namespace GameZone.Models
{
    public class Game : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string Developer { get; set; }
        public required string Publisher { get; set; }
        public List<Platform> Platforms { get; set; } = new List<Platform>();
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public AgeRating AgeRating { get; set; }
    }
}
