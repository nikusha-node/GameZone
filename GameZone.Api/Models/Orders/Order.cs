using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Api.Models;

public class Order
{
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string OrderNumber { get; set; } = string.Empty;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Precision(18, 2)]
    public decimal TotalAmount { get; set; }

   
    [Required, MaxLength(150)]
    public string ShippingFullName { get; set; } = string.Empty;

    [Required, MaxLength(250)]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string ShippingCity { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? ShippingPostalCode { get; set; }

    [Required, MaxLength(100)]
    public string ShippingCountry { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? ShippingPhone { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}