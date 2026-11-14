using GameZone.DTOs.Auth;
using GameZone.DTOs.User;
using GameZone.DependencyInjection.JWT;

namespace GameZone.DependencyInjection.Interfaces
{
    public interface IUserService
    {
        Task<bool> Register(RegisterRequest request);
        Task<UserToken?> Verify(string email, string code);
        Task<UserToken?> Login(string email, string password);
        Task<UserProfileResponse> GetProfile(int userId);
        Task<UserProfileResponse> UpdateProfile(int userId,UpdateProfileRequest request);
        Task<bool> ChangePassword(int userId,ChangePasswordRequest request);
    }
}