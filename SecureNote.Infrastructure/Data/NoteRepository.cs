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
            return await _dbContext.Notes
                .Where(note => note.UserId == userId)
                .Include(note => note.Category) 
                .ToListAsync();
        }
    }
}