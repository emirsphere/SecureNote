using System;
using System.Threading.Tasks;
using SecureNote.Application.DTOs;
using SecureNote.Application.Exceptions;
using SecureNote.Application.Interfaces;
using SecureNote.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SecureNote.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<UserDto> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email!);
            if (existingUser != null)
            {
                // ✅ ValidationException kullan
                throw new ValidationException("Bu e-posta adresi zaten kullanımda.");
            }

            var passwordHash = _passwordHasher.Hash(request.Password!);

            var newUser = new User
            {
                Username = request.Username!,
                Email = request.Email!,
                PasswordHash = passwordHash
            };

            await _userRepository.AddAsync(newUser);
            return new UserDto
            {
                Id = newUser.Id,
                Username = newUser.Username!,
                Email = newUser.Email!,
                CreatedOn = DateTime.UtcNow,
            };
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                // ✅ ValidationException kullan
                throw new ValidationException("E-posta veya şifre hatalı.");
            }

            var isPasswordValid = _passwordHasher.Verify(password, user.PasswordHash!);
            if (!isPasswordValid)
            {
                // ✅ ValidationException kullan
                throw new ValidationException("E-posta veya şifre hatalı.");
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.Username!)
            };

            var secretKey = _configuration["JwtSettings:SecretKey"]
                ?? throw new InvalidOperationException("JwtSettings:SecretKey is missing in appsettings.json");

            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var durationInMinutes = double.Parse(_configuration["JwtSettings:DurationInMinutes"] ?? "60");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(durationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}