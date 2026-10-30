
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Validators;

public static class CartItemValidator
{
    public static List<ValidationResult> Validate(CartItem item)
    {
        var results = new List<ValidationResult>();

        if (item.Quantity <= 0)
            results.Add(new ValidationResult(
                "Quantity must be greater than zero."));

        if (item.CartId <= 0)
            results.Add(new ValidationResult("Cart ID is required."));

        if (item.ProductId <= 0)
            results.Add(new ValidationResult("Product ID is required."));

        return results;
    }
}
