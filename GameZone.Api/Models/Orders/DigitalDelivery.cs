using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Models;
public class DigitalDelivery
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string LicenseKey { get; set; } = string.Empty;

    public DateTime? DeliveredAtUtc { get; set; }

    public int OrderItemId { get; set; }
    public OrderItem OrderItem { get; set; } = null!;
}