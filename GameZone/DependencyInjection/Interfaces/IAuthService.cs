using GameZone.DTOs.Auth;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequest request);

        Task<string> VerifyEmailAsync(VerifyEmailRequest request);

        Task<string> LoginAsync(LoginRequest request);
    }
}