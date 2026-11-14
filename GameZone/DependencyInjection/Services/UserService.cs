using AutoMapper;
using Microsoft.EntityFrameworkCore;
using GameZone.Data;
using GameZone.Models;
using GameZone.Enums;
using GameZone.DTOs.Auth;
using GameZone.DTOs.User;
using GameZone.DependencyInjection.Interfaces;
using GameZone.DependencyInjection.JWT;
using GameZone.DependencyInjection.SMTP;
using GameZone.Exceptions;

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

        public async Task<UserProfileResponse> GetProfile(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            return new UserProfileResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsVerified = user.IsVerified
            };
        }


        public async Task<bool> ChangePassword(
            int userId,
            ChangePasswordRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            bool currentPasswordIsValid =
                BCrypt.Net.BCrypt.Verify(
                    request.CurrentPassword,
                    user.PasswordHash);

            if (!currentPasswordIsValid)
                throw new BadRequestException(
                    "Current password is incorrect.");

            if (request.NewPassword == request.CurrentPassword)
                throw new BadRequestException(
                    "New password must be different from current password.");

            user.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<UserProfileResponse> UpdateProfile(
            int userId,
            UpdateProfileRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            if (string.IsNullOrWhiteSpace(request.FirstName))
                throw new BadRequestException("First name is required.");

            if (string.IsNullOrWhiteSpace(request.LastName))
                throw new BadRequestException("Last name is required.");

            user.FirstName = request.FirstName.Trim();
            user.LastName = request.LastName.Trim();
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UserProfileResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsVerified = user.IsVerified
            };
        }
    }
}
