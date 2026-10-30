
using GameZone.Api.Enum;
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Validators;

public static class OrderValidator
{
    public static List<ValidationResult> Validate(Order order)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(order.OrderNumber))
            results.Add(new ValidationResult("Order number is required."));
        else if (order.OrderNumber.Length > 30)
            results.Add(new ValidationResult("Order number cannot exceed 30 characters."));

        if (!Enum.IsDefined(typeof(OrderStatus), order.Status))
            results.Add(new ValidationResult("Invalid order status."));

        if (order.TotalAmount < 0)
            results.Add(new ValidationResult("Total amount cannot be negative."));

        if (string.IsNullOrWhiteSpace(order.ShippingFullName))
            results.Add(new ValidationResult("Shipping full name is required."));
        else if (order.ShippingFullName.Length > 150)
            results.Add(new ValidationResult("Shipping name cannot exceed 150 characters."));

        if (string.IsNullOrWhiteSpace(order.ShippingAddress))
            results.Add(new ValidationResult("Shipping address is required."));
        else if (order.ShippingAddress.Length > 250)
            results.Add(new ValidationResult("Shipping address cannot exceed 250 characters."));

        if (string.IsNullOrWhiteSpace(order.ShippingCity))
            results.Add(new ValidationResult("Shipping city is required."));
        else if (order.ShippingCity.Length > 100)
            results.Add(new ValidationResult("Shipping city cannot exceed 100 characters."));

        if (order.ShippingPostalCode?.Length > 20)
            results.Add(new ValidationResult("Postal code cannot exceed 20 characters."));

        if (string.IsNullOrWhiteSpace(order.ShippingCountry))
            results.Add(new ValidationResult("Shipping country is required."));
        else if (order.ShippingCountry.Length > 100)
            results.Add(new ValidationResult("Shipping country cannot exceed 100 characters."));

        if (order.ShippingPhone?.Length > 30)
            results.Add(new ValidationResult("Shipping phone cannot exceed 30 characters."));

        if (string.IsNullOrWhiteSpace(order.UserId))
            results.Add(new ValidationResult("User ID is required."));

        if (order.Items == null || order.Items.Count == 0)
            results.Add(new ValidationResult("Order must contain at least one item."));

        return results;
    }
}
