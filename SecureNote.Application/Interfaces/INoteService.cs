using System.Threading.Tasks;
using SecureNote.Application.DTOs;
using SecureNote.Domain.Entities;


namespace SecureNote.Application.Interfaces
{
    public interface INoteService
    {
        Task<ResponseNote> CreateNoteAsync(NoteDto request, Guid userId);
        Task<IReadOnlyList<ResponseNote>> GetNotesByUserIdAsync(Guid userId);
        Task UpdateNoteAsync(UpdateNoteRequest request, Guid userId);
        Task DeleteNoteAsync(Guid noteId, Guid userId);
    }
}
