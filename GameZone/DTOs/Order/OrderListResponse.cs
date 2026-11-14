namespace GameZone.DTOs.Order;

public class OrderListResponse
{
    public List<OrderResponse> Orders { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
