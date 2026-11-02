using AutoMapper;
using Microsoft.EntityFrameworkCore;

using GameZone.Data;
using GameZone.Models;
using GameZone.Enums;
using GameZone.DTOs.Auth;
using GameZone.DependencyInjection.Interfaces;
using GameZone.DependencyInjection.JWT;
using GameZone.DependencyInjection.SMTP;

namespace GameZone.DependencyInjection.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwt;

        public UserService(
            DataContext context,
            IMapper mapper,
            IJwtService jwt)
        {
            _context = context;
            _mapper = mapper;
            _jwt = jwt;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var userExists = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (userExists != null)
                return false;

            var user = _mapper.Map<User>(request);

            user.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(request.Password);

            user.Role = Role.User;
            user.IsVerified = false;

            await SMTPService.SendEmail(user);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return true;
        }

        // VERIFY EMAIL
        public async Task<UserToken?> Verify(string email, string code)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == email &&
                    u.Code == code);

            if (user == null || user.IsVerified)
                return null;

            user.IsVerified = true;
            user.Code = null;

            await _context.SaveChangesAsync();

            return _jwt.GetUserToken(user);
        }

        // LOGIN
        public async Task<UserToken?> Login(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            if (!user.IsVerified)
                return null;

            bool passwordIsValid =
                BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!passwordIsValid)
                return null;

            return _jwt.GetUserToken(user);
        }
    }
}
