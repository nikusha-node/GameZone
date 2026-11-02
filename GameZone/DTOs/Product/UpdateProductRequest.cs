
using GameZone.Enums;

namespace GameZone.DTOs.Product
{
    public class UpdateProductRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public int BrandId { get; set; }
        public Category Category { get; set; }
    }
}
