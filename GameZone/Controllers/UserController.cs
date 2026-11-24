
using GameZone.DependencyInjection.Interfaces;
using GameZone.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id))
                throw new UnauthorizedAccessException(
                    "Invalid user token.");

            return id;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _userService.GetProfile(GetUserId());

            return Ok(profile);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(
            UpdateProfileRequest request)
        {
            var profile = await _userService.UpdateProfile(
                GetUserId(), request);

            return Ok(profile);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordRequest request)
        {
            await _userService.ChangePassword(
                GetUserId(), request);

            return Ok(new
            {
                Message = "Password changed successfully."
            });
        }
    }
}
