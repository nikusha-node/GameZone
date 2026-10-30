
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Validators;

public static class CategoryValidator
{
    public static List<ValidationResult> Validate(Category category)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(category.Name))
            results.Add(new ValidationResult("Category name is required."));

        else if (category.Name.Length > 100)
            results.Add(new ValidationResult("Name cannot exceed 100 characters."));

        if (string.IsNullOrWhiteSpace(category.Slug))
            results.Add(new ValidationResult("Slug is required."));

        else if (category.Slug.Length > 120 ||
                 !System.Text.RegularExpressions.Regex.IsMatch(
                     category.Slug, @"^[a-z0-9]+(?:-[a-z0-9]+)*$"))
            results.Add(new ValidationResult("Invalid slug format."));

        if (category.Description?.Length > 500)
            results.Add(new ValidationResult("Description cannot exceed 500 characters."));

        return results;
    }
}
