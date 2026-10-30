
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Validators;

public static class CartValidator
{
    public static List<ValidationResult> Validate(Cart cart)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(cart.UserId))
            results.Add(new ValidationResult("User ID is required."));

        if (cart.Items == null)
            results.Add(new ValidationResult("Cart items cannot be null."));

        if (cart.UpdatedAtUtc.HasValue &&
            cart.UpdatedAtUtc.Value < cart.CreatedAtUtc)
            results.Add(new ValidationResult(
                "Updated date cannot be earlier than created date."));

        return results;
    }
}
