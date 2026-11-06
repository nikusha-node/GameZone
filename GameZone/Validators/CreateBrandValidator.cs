using FluentValidation;
using GameZone.DTOs.Brand;

namespace GameZone.Validators
{
    public class CreateBrandValidator : AbstractValidator<CreateBrandRequest>
    {
        public CreateBrandValidator()
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
                .MaximumLength(500)
                .Must(BeAValidUrl)
                .WithMessage("Please enter a valid logo URL.");
        }

        private static bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var result)
                && (result.Scheme == Uri.UriSchemeHttp
                    || result.Scheme == Uri.UriSchemeHttps);
        }
    }
}