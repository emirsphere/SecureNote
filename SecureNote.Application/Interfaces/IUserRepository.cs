using System.Threading.Tasks;
using SecureNote.Domain.Entities;

namespace SecureNote.Application.Interfaces
{
    // IAsyncRepository<User> diyerek standart 5 metodu (Ekle, Sil vs.) otomatik miras aldık.
    // Ekstra kod yazmaya gerek yok!
    public interface IUserRepository : IAsyncRepository<User>
    {
        // Standart dışı, sadece User'a özel bir ihtiyaç:
        Task<User?> GetByEmailAsync(string email);
    }
}