using GameZone.Enums;

namespace GameZone.Models
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public required Brand Brand { get; set; }
        public int BrandId { get; set; }
        public Category Category { get; set; }
    }
}
