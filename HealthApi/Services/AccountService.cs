using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using HealthApi.Entities;
using HealthApi.Exceptions;
using HealthApi.Models;

namespace HealthApi.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(User user, string providedPassord);
        User AuthenticateUser(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly HealthDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly Setting _settings;

        public AccountService(HealthDbContext context, IPasswordHasher<User> passwordHasher, Setting settings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _settings = settings;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                RoleId = dto.RoleId
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.Password = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
        public User AuthenticateUser(LoginDto dto)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);
        }
        public string GenerateJwt(User user, string providedPassord)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, providedPassord);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),

            };

            if (!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(
                    new Claim("Nationality", user.Nationality)
                    );
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_settings.JwtExpireDays);

            var token = new JwtSecurityToken(_settings.JwtIssuer,
                _settings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);

        }
    }
}
