using FluentValidation;
using GameZone.DTOs.Platform;

namespace GameZone.Validators
{
    public class CreatePlatformValidator : AbstractValidator<CreatePlatformRequest>
    {
        public CreatePlatformValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Manufacturer)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
