
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Validators;

public static class WishlistItemValidator
{
    public static List<ValidationResult> Validate(WishlistItem item)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(item.UserId))
            results.Add(new ValidationResult("User ID is required."));

        if (item.ProductId <= 0)
            results.Add(new ValidationResult("Product ID is required."));

        if (item.AddedAtUtc > DateTime.UtcNow)
            results.Add(new ValidationResult(
                "Added date cannot be in the future."));

        return results;
    }
}
