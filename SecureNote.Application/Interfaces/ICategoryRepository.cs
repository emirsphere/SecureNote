using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecureNote.Domain.Entities;

namespace SecureNote.Application.Interfaces
{
    public interface ICategoryRepository: IAsyncRepository<Category>
    {
        Task<IReadOnlyList<Category>> GetCategoriesByUserIdAsync(Guid userId);
        Task<bool> IsCategoryNameExistsAsync(string categoryName, Guid userId);
    }
}