using System.Threading.Tasks;
using SecureNote.Application.DTOs;
using SecureNote.Domain.Entities;

namespace SecureNote.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request, Guid userId);
        Task<IReadOnlyList<CategoryDto>> GetCategoriesByUserIdAsync(Guid userId);
        Task UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequest request, Guid userId);
        Task DeleteCategoryAsync(Guid categoryId, Guid userId);
    }
}
