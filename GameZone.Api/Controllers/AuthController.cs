using System.Security.Claims;
using GameZone.Api.DTOs;
using GameZone.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        => Ok(await _auth.RegisterAsync(dto));

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        => Ok(await _auth.LoginAsync(dto));

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me() => Ok(new
    {
        Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
        Email = User.FindFirstValue(ClaimTypes.Email),
        Name = User.FindFirstValue(ClaimTypes.Name),
        Roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value)
    });
}