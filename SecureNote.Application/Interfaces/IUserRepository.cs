using System.Threading.Tasks;
using SecureNote.Domain.Entities;

namespace SecureNote.Application.Interfaces
{
    
    public interface IUserRepository : IAsyncRepository<User>
    {

        Task<User?> GetByEmailAsync(string email);
    }
}