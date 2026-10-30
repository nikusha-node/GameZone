
using GameZone.Api.Enum;
using GameZone.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GameZone.Api.Validators;

public static class UserValidator
{
    public static List<ValidationResult> Validate(User user)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(user.FullName))
            results.Add(new ValidationResult("Full name is required."));
        else if (user.FullName.Length > 150)
            results.Add(new ValidationResult("Full name cannot exceed 150 characters."));

        if (string.IsNullOrWhiteSpace(user.Email))
            results.Add(new ValidationResult("Email is required."));
        else if (user.Email.Length > 254 ||
                 !new EmailAddressAttribute().IsValid(user.Email))
            results.Add(new ValidationResult("Invalid email address."));

        if (string.IsNullOrWhiteSpace(user.Password))
            results.Add(new ValidationResult("Password is required."));
        else if (user.Password.Length < 8)
            results.Add(new ValidationResult("Password must contain at least 8 characters."));
        else if (!Regex.IsMatch(user.Password, @"[A-Z]") ||
                 !Regex.IsMatch(user.Password, @"[a-z]") ||
                 !Regex.IsMatch(user.Password, @"[0-9]"))
            results.Add(new ValidationResult(
                "Password must contain uppercase, lowercase and numeric characters."));

        if (string.IsNullOrWhiteSpace(user.PhoneNumber))
            results.Add(new ValidationResult("Phone number is required."));
        else if (!Regex.IsMatch(user.PhoneNumber, @"^\+?[0-9]{7,15}$"))
            results.Add(new ValidationResult("Invalid phone number."));

        if (!Enum.IsDefined(typeof(Role), user.Role))
            results.Add(new ValidationResult("Invalid user role."));

        if (user.code?.Length > 10)
            results.Add(new ValidationResult("Verification code cannot exceed 10 characters."));

        return results;
    }
}
