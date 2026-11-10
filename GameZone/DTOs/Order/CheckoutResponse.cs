namespace GameZone.DTOs.Order
{
    public class CheckoutResponse
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
    }
}