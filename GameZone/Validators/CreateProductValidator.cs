using FluentValidation;
using GameZone.DTOs.Product;

namespace GameZone.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(100);

            RuleFor(p => p.Description)
                .MaximumLength(1000);

            RuleFor(p => p.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");

            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock quantity cannot be negative.");

            RuleFor(p => p.BrandId)
                .GreaterThan(0)
                .WithMessage("A valid brand is required.");

            RuleFor(p => p.Category)
                .IsInEnum()
                .WithMessage("Invalid product category.");

            RuleFor(p => p.ImageUrl)
                .MaximumLength(500);
        }
    }
}