using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecureNote.Application.Interfaces;
using SecureNote.Domain.Entities;

namespace SecureNote.Infrastructure.Data
{
    
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(SecureNoteDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {        
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}