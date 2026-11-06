namespace GameZone.DTOs.Brand
{
    public class CreateBrandRequest
    {
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required string Description { get; set; }
        public required string LogoUrl { get; set; }
    }
}