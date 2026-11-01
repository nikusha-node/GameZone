using GameZone.Models;
using GameZone.DTOs.Auth;

namespace GameZone.DependencyInjection.JWT
{
    public interface IJwtService
    {
        UserToken GetUserToken(User user);
    }
}