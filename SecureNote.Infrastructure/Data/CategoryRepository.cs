using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecureNote.Application.Interfaces;
using SecureNote.Domain.Entities;

namespace SecureNote.Infrastructure.Data
{
    public class CategoryRepository: EfRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(SecureNoteDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Category>> GetCategoriesByUserIdAsync(Guid userId)
        {
            return await _dbContext.Categories
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task<bool> IsCategoryNameExistsAsync(string categoryName, Guid userId)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.UserId == userId && c.CategoryName == categoryName);
        }
    }
}
