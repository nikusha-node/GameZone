
using FluentValidation;
using GameZone.DTOs.Game;

namespace GameZone.Validators
{
    public class CreateGameValidator : AbstractValidator<CreateGameRequest>
    {
        public CreateGameValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.ReleaseDate)
                .NotEmpty();

            RuleFor(x => x.Developer)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Publisher)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.PlatformIds)
                .NotNull()
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Platform IDs must not contain duplicates.");
        }
    }
}
