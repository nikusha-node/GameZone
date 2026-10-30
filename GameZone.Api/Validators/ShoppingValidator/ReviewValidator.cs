
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Validators;

public static class ReviewValidator
{
    public static List<ValidationResult> Validate(Review review)
    {
        var results = new List<ValidationResult>();

        if (review.Rating < 1 || review.Rating > 5)
            results.Add(new ValidationResult(
                "Rating must be between 1 and 5."));

        if (review.Title?.Length > 100)
            results.Add(new ValidationResult(
                "Review title cannot exceed 100 characters."));

        if (review.Comment?.Length > 2000)
            results.Add(new ValidationResult(
                "Review comment cannot exceed 2000 characters."));

        if (review.ProductId <= 0)
            results.Add(new ValidationResult("Product ID is required."));

        if (string.IsNullOrWhiteSpace(review.UserId))
            results.Add(new ValidationResult("User ID is required."));

        return results;
    }
}
