
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Validators;

public static class OrderItemValidator
{
    public static List<ValidationResult> Validate(OrderItem item)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(item.ProductName))
            results.Add(new ValidationResult("Product name is required."));
        else if (item.ProductName.Length > 200)
            results.Add(new ValidationResult("Product name cannot exceed 200 characters."));

        if (item.UnitPrice <= 0)
            results.Add(new ValidationResult("Unit price must be greater than zero."));

        if (item.Quantity <= 0)
            results.Add(new ValidationResult("Quantity must be greater than zero."));

        if (item.OrderId <= 0)
            results.Add(new ValidationResult("Order ID is required."));

        if (item.ProductId <= 0)
            results.Add(new ValidationResult("Product ID is required."));

        return results;
    }
}
