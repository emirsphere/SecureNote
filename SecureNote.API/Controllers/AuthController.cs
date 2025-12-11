using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SecureNote.Application.DTOs;
using SecureNote.Application.Interfaces;

namespace SecureNote.API.Controllers
{
    // [ApiController]: Bu sınıfın bir API olduğunu belirtir (Validasyonları otomatik yapar).
    // [Route]: Adresi belirler -> "https://localhost:xxxx/api/auth"
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // Constructor Injection:
        // Program.cs'de tanıttığımız AuthService buraya otomatik gelir.
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _authService.RegisterAsync(request);

                // 201 Created döner ve oluşturulan kullanıcıyı gösterir
                return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                // Basit hata yönetimi. Gerçek projede global exception handler kullanırız.
                return BadRequest(new { message = ex.Message });
            }
        }
        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginAsync(request.Email, request.Password);

                // Başarılı olursa 200 OK ve Token döner
                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                // Hatalı giriş denemesi -> 401 Unauthorized
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}