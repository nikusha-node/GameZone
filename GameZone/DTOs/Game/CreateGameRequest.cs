namespace GameZone.DTOs.Game
{
    public class CreateGameRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string Developer { get; set; }
        public required string Publisher { get; set; }
        public List<int> PlatformIds { get; set; } = new List<int>();
        public int ProductId { get; set; }
    }
}
