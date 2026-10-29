using GameZone.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace GameZone.Api.Helpers;
public class BCryptPasswordHasher : IPasswordHasher<ApplicationUser>
{
    private readonly PasswordHasher<ApplicationUser> _legacy = new();

    public string HashPassword(ApplicationUser user, string password)
        => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

    public PasswordVerificationResult VerifyHashedPassword(
        ApplicationUser user, string hashedPassword, string providedPassword)
    {
        if (hashedPassword.StartsWith("$2"))
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword)
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }

        var result = _legacy.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Failed
            ? PasswordVerificationResult.Failed
            : PasswordVerificationResult.SuccessRehashNeeded;
    }
}