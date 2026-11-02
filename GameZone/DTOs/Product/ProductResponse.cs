
using GameZone.Enums;

namespace GameZone.DTOs.Product
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
