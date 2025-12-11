using SecureNote.Application.Interfaces;
using BCrypt.Net; // NuGet paketinden geliyor

namespace SecureNote.Infrastructure.Services
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            // MÜHENDİSLİK DOKUNUŞU:
            // BCrypt.HashPassword metodu, arka planda OTOMATİK olarak rastgele bir "Salt" (Tuz) üretir.
            // Bu tuzu hash sonucunun içine gizler. 
            // Yani "1234" için her çağırdığında farklı bir sonuç üretir ama Verify ederken bunu anlar.
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string passwordHash)
        {
            // Kullanıcının girdiği şifreyi, veritabanındaki tuzlu hash ile matematiksel olarak doğrular.
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}