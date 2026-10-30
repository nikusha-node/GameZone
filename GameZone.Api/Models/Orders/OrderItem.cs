using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Api.Models;
public class OrderItem
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Precision(18, 2)]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal LineTotal => UnitPrice * Quantity;

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public DigitalDelivery? DigitalDelivery { get; set; }
}