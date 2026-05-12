using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//redis için interface oluşturduk
namespace SecureNote.Application.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null);// t value diyerek herhangi bir tipte veri saklamayı sağlıyoruz.
        Task RemoveAsync(string key);
    }
}
