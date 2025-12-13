using System;
using System.Collections.Generic;
using System.Linq; // Select (Dönüşüm) için gerekli
using System.Threading.Tasks;
using SecureNote.Application.DTOs;
using SecureNote.Application.Interfaces;
using SecureNote.Domain.Entities;

namespace SecureNote.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IEncryptionService _encryptionService;

        public NoteService(INoteRepository noteRepository, IEncryptionService encryptionService)
        {
            _noteRepository = noteRepository;
            _encryptionService = encryptionService;
        }

        public async Task<ResponseNote> CreateNoteAsync(NoteDto request, Guid userId)
        {
            var encryptedContent = _encryptionService.Encrypt(request.Content);

            var newNote = new Note
            {
                Title = request.Title,
                Content = encryptedContent,
                UserId = userId,
            };
            await _noteRepository.AddAsync(newNote);

            return new ResponseNote
            {
                Id = newNote.Id,
                Title = newNote.Title,
                Content = request.Content, // Decrypted content
                CreatedOn = newNote.CreatedOn
            };
        }

        public async Task<IReadOnlyList<ResponseNote>> GetNotesByUserIdAsync(Guid userId)
        {
            var encryptedNotes = await _noteRepository.GetNotesByUserIdAsync(userId);

            var noteResponses = encryptedNotes.Select(note =>
            {
                string decryptedContent;
                try
                {
                    decryptedContent = _encryptionService.Decrypt(note.Content);
                }
                catch
                {
                    decryptedContent = "[Şifre Çözülemedi / Veri Bozuk]";
                }

                return new ResponseNote
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = decryptedContent,
                    CreatedOn = note.CreatedOn
                };
            }).ToList();

            return noteResponses;
        }

        public async Task UpdateNoteAsync(UpdateNoteRequest request, Guid userId)
        {
            var note = await _noteRepository.GetByIdAsync(request.Id);
            if (note == null) throw new Exception("Not bulunamadı.");
            if (note.UserId != userId) throw new Exception("Bu işlem için yetkiniz yok.");

            note.Title = request.Title;
            note.Content = _encryptionService.Encrypt(request.Content);
            await _noteRepository.UpdateAsync(note);
        }

        public async Task DeleteNoteAsync (Guid noteId, Guid userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId);
            if (note == null) throw new Exception("Not bulunamadı.");
            if(note.UserId != userId) throw new Exception("Bu işlem için yetkiniz yok.");

            await _noteRepository.DeleteAsync(note);
        }
    }
}
