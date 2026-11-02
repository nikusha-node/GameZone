using GameZone.DTOs.Auth;
using GameZone.DependencyInjection.JWT;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IUserService
    {
        Task<bool> Register(RegisterRequest request);

        Task<UserToken?> Verify(string email, string code);

        Task<UserToken?> Login(string email, string password);
    }
}