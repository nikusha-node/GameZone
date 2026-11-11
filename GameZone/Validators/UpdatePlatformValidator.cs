using FluentValidation;
using GameZone.DTOs.Platform;

namespace GameZone.Validators
{
    public class UpdatePlatformValidator : AbstractValidator<UpdatePlatformRequest>
    {
        public UpdatePlatformValidator()
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
