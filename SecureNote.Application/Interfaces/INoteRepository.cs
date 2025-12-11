using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecureNote.Domain.Entities;

namespace SecureNote.Application.Interfaces
{
    public interface INoteRepository : IAsyncRepository<Note>
    {
        // Belirli bir kullanıcının tüm notlarını getir
        Task<IReadOnlyList<Note>> GetNotesByUserIdAsync(Guid userId);
    }
}