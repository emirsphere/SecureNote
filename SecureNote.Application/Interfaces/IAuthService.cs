using System.Threading.Tasks;
using SecureNote.Application.DTOs;
using SecureNote.Domain.Entities;

namespace SecureNote.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(string email, string password);
    }
}