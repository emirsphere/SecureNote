using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SecureNote.Application.DTOs;
using SecureNote.Application.Interfaces;

namespace SecureNote.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Kullanıcı kaydı oluşturur
        /// </summary>
        /// <param name="request">Kayıt bilgileri</param>
        /// <returns>Oluşturulan kullanıcı</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // ✅ Try-catch kaldırıldı - Middleware yakalayacak
            var result = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
        }

        /// <summary>
        /// Kullanıcı giriş bilgileriyle token oluşturur
        /// </summary>
        /// <param name="request">Giriş bilgileri (email, şifre)</param>
        /// <returns>JWT token</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // ✅ Try-catch kaldırıldı - Middleware yakalayacak
            var token = await _authService.LoginAsync(request.Email, request.Password);
            return Ok(new { token = token });
        }

        /// <summary>
        /// Giriş isteği DTO'su
        /// </summary>
        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}