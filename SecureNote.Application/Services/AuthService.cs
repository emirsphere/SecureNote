using System;
using System.Threading.Tasks;
using SecureNote.Application.DTOs;
using SecureNote.Application.Interfaces;
using SecureNote.Domain.Entities;
// JWT Kütüphaneleri
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration; // AppSettings okumak için
using Microsoft.IdentityModel.Tokens;

namespace SecureNote.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration; // AppSettings'e erişmek için

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<UserDto> RegisterAsync(RegisterRequest request)
        {
            // Validasyon katmanı (FluentValidation) Email'in null olmadığını garanti eder.
            // Derleyiciye "!" ile bunu bildiriyoruz.
            var existingUser = await _userRepository.GetByEmailAsync(request.Email!);
            if (existingUser != null)
            {
                throw new Exception("Bu e-posta adresi zaten kullanımda.");
            }

            // Password için de aynı garanti geçerli.
            var passwordHash = _passwordHasher.Hash(request.Password!);

            // Entity Oluşturma
            var newUser = new User
            {
                Username = request.Username!,
                Email = request.Email!,
                PasswordHash = passwordHash
            };

            // 4. Kayıt
            await _userRepository.AddAsync(newUser);
            return new UserDto
            {
                Id = newUser.Id,
                Username = newUser.Username!,
                Email = newUser.Email!,
                CreatedOn = DateTime.UtcNow,
            };


        }
             // User entity'sini DTO'ya dönüştürme

        // --- LOGIN METODU ---
        public async Task<string> LoginAsync(string email, string password)
        {
            // 1. Kullanıcıyı Bul
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                // Güvenlik: Kullanıcı bulunamadı demek yerine genel hata veriyoruz.
                throw new Exception("E-posta veya şifre hatalı.");
            }

            // 2. Şifreyi Doğrula
            // User entity'sindeki PasswordHash veritabanından zorunlu (required) geldiği için null olamaz.
            // Ama yine de defensive coding adına null check yapılabilir veya ! kullanılabilir.
            var isPasswordValid = _passwordHasher.Verify(password, user.PasswordHash!);
            if (!isPasswordValid)
            {
                throw new Exception("E-posta veya şifre hatalı.");
            }

            // 3. Token Üret (Bilet Kesimi)
            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            // Token'ın içine gömeceğimiz bilgiler (Claims)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!), // Email null olamaz
                new Claim(ClaimTypes.Name, user.Username!) // Username null olamaz
            };

            // Güvenli Konfigürasyon Okuma (Null Check)
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