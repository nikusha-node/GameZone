using Microsoft.AspNetCore.Mvc;
using GameZone.DTOs.Auth;
using GameZone.DependencyInjection.Interfaces;

namespace GameZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _userService.Register(request);

            if (!result)
                return BadRequest("Email is already registered.");

            return Ok("Registration successful. Check your email for the verification code.");
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(VerifyEmailRequest request)
        {
            var result = await _userService.Verify(
                request.Email,
                request.Code);

            if (result == null)
                return BadRequest("Invalid email or verification code.");

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _userService.Login(
                request.Email,
                request.Password);

            if (result == null)
                return Unauthorized("Invalid credentials or unverified account.");

            return Ok(result);
        }
    }
}