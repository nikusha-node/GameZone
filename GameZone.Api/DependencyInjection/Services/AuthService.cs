using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameZone.Api.Configurations;
using GameZone.Api.DTOs;
using GameZone.Api.Exceptions;
using GameZone.Api.Interfaces;
using GameZone.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GameZone.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly JwtSettings _jwt;

    public AuthService(UserManager<ApplicationUser> users, IOptions<JwtSettings> jwt)
    {
        _users = users;
        _jwt = jwt.Value;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var email = dto.Email.Trim();

        if (await _users.FindByEmailAsync(email) is not null)
            throw new ConflictException("An account with this email already exists.");

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = dto.FullName.Trim()
        };

        var result = await _users.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new BadRequestException(string.Join(" ", result.Errors.Select(e => e.Description)));

        await _users.AddToRoleAsync(user, AppRoles.Customer);

        return await BuildResponseAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _users.FindByEmailAsync(dto.Email.Trim());

        if (user is null || !await _users.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedException("Invalid email or password.");

        return await BuildResponseAsync(user);
    }

    private async Task<AuthResponseDto> BuildResponseAsync(ApplicationUser user)
    {
        var roles = (await _users.GetRolesAsync(user)).ToList();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.FullName)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expires,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName,
            Roles = roles
        };
    }
}