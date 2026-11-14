namespace GameZone.DTOs.Product
{
    public class ProductListResponse
    {
        public List<ProductResponse> Products { get; set; } = new();

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}