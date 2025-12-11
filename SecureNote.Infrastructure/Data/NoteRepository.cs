using System;
using System.Collections.Generic;
using System.Linq; // Where sorgusu için gerekli
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecureNote.Application.Interfaces;
using SecureNote.Domain.Entities;

namespace SecureNote.Infrastructure.Data
{
    public class NoteRepository : EfRepository<Note>, INoteRepository
    {
        public NoteRepository(SecureNoteDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Note>> GetNotesByUserIdAsync(Guid userId)
        {
            // SQL Karşılığı: SELECT * FROM Notes WHERE UserId = '...'
            return await _dbContext.Notes
                .Where(note => note.UserId == userId)
                .ToListAsync();
        }
    }
}