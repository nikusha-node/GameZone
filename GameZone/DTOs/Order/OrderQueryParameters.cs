using GameZone.Enums;

namespace GameZone.DTOs.Order;

public class OrderQueryParameters
{
    public OrderStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
