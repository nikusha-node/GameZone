namespace GameZone.DTOs.Platform
{
    public class PlatformResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Manufacturer { get; set; }
        public int GameCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
