
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Validators;

public static class DigitalDeliveryValidator
{
    public static List<ValidationResult> Validate(DigitalDelivery delivery)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(delivery.LicenseKey))
            results.Add(new ValidationResult("License key is required."));
        else if (delivery.LicenseKey.Length > 200)
            results.Add(new ValidationResult("License key cannot exceed 200 characters."));

        if (delivery.OrderItemId <= 0)
            results.Add(new ValidationResult("Order item ID is required."));

        if (delivery.DeliveredAtUtc.HasValue &&
            delivery.DeliveredAtUtc.Value > DateTime.UtcNow)
            results.Add(new ValidationResult("Delivery date cannot be in the future."));

        return results;
    }
}
