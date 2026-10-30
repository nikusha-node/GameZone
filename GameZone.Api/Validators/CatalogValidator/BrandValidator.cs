
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Validators;

public static class BrandValidator
{
    public static List<ValidationResult> Validate(Brand brand)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(brand.Name))
            results.Add(new ValidationResult(
                "Brand name is required.", new[] { nameof(brand.Name) }));

        else if (brand.Name.Length > 100)
            results.Add(new ValidationResult(
                "Brand name cannot exceed 100 characters.",
                new[] { nameof(brand.Name) }));

        if (string.IsNullOrWhiteSpace(brand.Slug))
            results.Add(new ValidationResult(
                "Slug is required.", new[] { nameof(brand.Slug) }));

        else if (brand.Slug.Length > 120 ||
                 !System.Text.RegularExpressions.Regex.IsMatch(
                     brand.Slug, @"^[a-z0-9]+(?:-[a-z0-9]+)*$"))
            results.Add(new ValidationResult(
                "Invalid slug format.", new[] { nameof(brand.Slug) }));

        if (brand.LogoUrl?.Length > 500)
            results.Add(new ValidationResult(
                "Logo URL cannot exceed 500 characters.",
                new[] { nameof(brand.LogoUrl) }));

        return results;
    }
}
