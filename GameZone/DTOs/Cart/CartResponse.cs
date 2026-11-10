namespace GameZone.DTOs.Cart
{
    public class CartResponse
    {
        public int Id { get; set; }
        public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();
        public decimal TotalPrice { get; set; }
    }

    public class CartItemResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}