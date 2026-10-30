
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Validators;

public static class GameValidator
{
    public static List<ValidationResult> Validate(Game game)
    {
        var results = new List<ValidationResult>();

        if (game.Genre?.Length > 100)
            results.Add(new ValidationResult("Genre cannot exceed 100 characters."));

        if (game.Developer?.Length > 150)
            results.Add(new ValidationResult("Developer cannot exceed 150 characters."));

        if (game.Publisher?.Length > 150)
            results.Add(new ValidationResult("Publisher cannot exceed 150 characters."));

        if (game.ReleaseDate.HasValue &&
            game.ReleaseDate.Value > DateOnly.FromDateTime(DateTime.UtcNow))
            results.Add(new ValidationResult("Release date cannot be in the future."));

        if (game.AgeRating?.Length > 20)
            results.Add(new ValidationResult("Age rating cannot exceed 20 characters."));

        if (game.ProductId <= 0)
            results.Add(new ValidationResult("Product ID is required."));

        if (game.Pictures.Any(string.IsNullOrWhiteSpace))
            results.Add(new ValidationResult("Picture URLs cannot be empty."));

        if (game.Pictures.Any(p => p.Length > 500))
            results.Add(new ValidationResult("Picture URL cannot exceed 500 characters."));

        return results;
    }
}
