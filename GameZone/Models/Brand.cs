namespace GameZone.Models
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
