namespace GameZone.Models
{
    public class Platform : BaseEntity
    {
        public required string Name { get; set; }
        public required string Manufacturer { get; set; }
        public List<Game> Games { get; set; } = new List<Game>();
    }
}
