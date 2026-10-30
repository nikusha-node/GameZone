
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GameZone.Api.Validators;

public static class ProductValidator
{
    public static List<ValidationResult> Validate(Product product)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(product.Name))
            results.Add(new ValidationResult("Product name is required."));
        else if (product.Name.Length > 200)
            results.Add(new ValidationResult("Name cannot exceed 200 characters."));

        if (string.IsNullOrWhiteSpace(product.Slug))
            results.Add(new ValidationResult("Slug is required."));
        else if (product.Slug.Length > 220 ||
                 !Regex.IsMatch(product.Slug, @"^[a-z0-9]+(?:-[a-z0-9]+)*$"))
            results.Add(new ValidationResult("Invalid slug format."));

        if (product.Description?.Length > 4000)
            results.Add(new ValidationResult("Description cannot exceed 4000 characters."));

        if (product.Price <= 0)
            results.Add(new ValidationResult("Price must be greater than zero."));

        if (product.DiscountPrice.HasValue)
        {
            if (product.DiscountPrice <= 0)
                results.Add(new ValidationResult("Discount price must be greater than zero."));

            if (product.DiscountPrice >= product.Price)
                results.Add(new ValidationResult(
                    "Discount price must be lower than regular price."));
        }

        if (product.StockQuantity < 0)
            results.Add(new ValidationResult("Stock quantity cannot be negative."));

        if (product.CategoryId <= 0)
            results.Add(new ValidationResult("Category is required."));

        if (product.BrandId.HasValue && product.BrandId <= 0)
            results.Add(new ValidationResult("Invalid brand ID."));

        return results;
    }
}
