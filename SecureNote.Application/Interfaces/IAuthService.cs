using System.Threading.Tasks;
using SecureNote.Application.DTOs;
using SecureNote.Domain.Entities;

namespace SecureNote.Application.Interfaces
{
    public interface IAuthService
    {
        // Kayıt olma işlemi. Geriye oluşturulan User'ı dönsün (veya sadece başarılı bilgisini).
        Task<User> RegisterAsync(RegisterRequest request);

        // Giriş yapma işlemi. Geriye JWT Token (string) dönecek.
        Task<string> LoginAsync(string email, string password);
    }
}