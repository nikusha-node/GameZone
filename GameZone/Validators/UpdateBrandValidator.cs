using FluentValidation;
using GameZone.DTOs.Brand;

namespace GameZone.Validators
{
    public class UpdateBrandValidator : AbstractValidator<UpdateBrandRequest>
    {
        public UpdateBrandValidator()
        {
            RuleFor(b => b.Name)
                .NotEmpty()
                .WithMessage("Brand name is required.")
                .MaximumLength(100);

            RuleFor(b => b.Slug)
                .NotEmpty()
                .WithMessage("Brand slug is required.")
                .MaximumLength(100)
                .Matches("^[a-z0-9-]+$")
                .WithMessage("Slug can contain only lowercase letters, numbers, and hyphens.");

            RuleFor(b => b.Description)
                .NotEmpty()
                .WithMessage("Brand description is required.")
                .MaximumLength(1000);

            RuleFor(b => b.LogoUrl)
                .NotEmpty()
                .WithMessage("Brand logo URL is required.")
                .MaximumLength(500);
        }

    }
}