using FluentValidation;
using GameZone.DTOs.User;

namespace GameZone.Validators.User
{
    public class ChangePasswordValidator
        : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("New password is required.")
                .MinimumLength(8)
                .WithMessage("Password must contain at least 8 characters.")
                .Matches("[A-Z]")
                .WithMessage("Password must contain an uppercase letter.")
                .Matches("[a-z]")
                .WithMessage("Password must contain a lowercase letter.")
                .Matches("[0-9]")
                .WithMessage("Password must contain a number.");

            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Password confirmation does not match.");
        }
    }
}
