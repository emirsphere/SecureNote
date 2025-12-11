using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecureNote.Application.Interfaces;
using SecureNote.Domain.Entities;

namespace SecureNote.Infrastructure.Data
{
    // Hem EfRepository'nin yeteneklerini alıyor (Miras), hem de IUserRepository sözleşmesini imzalıyor.
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(SecureNoteDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            // Veritabanında mail adresine göre arama yapar.
            // FirstOrDefaultAsync: Bulursa getirir, bulamazsa null döner.
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}