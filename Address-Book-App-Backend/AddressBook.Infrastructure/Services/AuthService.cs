using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Application.DTOs;
using AddressBook.Application.Interfaces;
using AddressBook.Domain.Entities;
using AddressBook.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AddressBook.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            
            if (_context.Users.Any(x => x.Email == dto.Email))
                throw new Exception("Email already registered");

            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return GenerateToken(user);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                throw new Exception("Invalid email or password");

            return GenerateToken(user);
        }

        private string GenerateToken(AppUser user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
