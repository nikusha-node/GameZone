
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Api.Validators;

public static class ProductSpecificationValidator
{
    public static List<ValidationResult> Validate(ProductSpecification specification)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(specification.Name))
            results.Add(new ValidationResult("Specification name is required."));
        else if (specification.Name.Length > 100)
            results.Add(new ValidationResult("Name cannot exceed 100 characters."));

        if (string.IsNullOrWhiteSpace(specification.Value))
            results.Add(new ValidationResult("Specification value is required."));
        else if (specification.Value.Length > 300)
            results.Add(new ValidationResult("Value cannot exceed 300 characters."));

        if (specification.SortOrder < 0)
            results.Add(new ValidationResult("Sort order cannot be negative."));

        if (specification.ProductId <= 0)
            results.Add(new ValidationResult("Product ID is required."));

        return results;
    }
}
