using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecureNote.Application.DTOs;
using SecureNote.Application.Exceptions;
using SecureNote.Application.Interfaces;
using SecureNote.Domain.Entities;

namespace SecureNote.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(request.CategoryName))
                throw new ValidationException("Kategori adı boş olamaz.");
            var categoryNameExist = await _categoryRepository.IsCategoryNameExistsAsync(request.CategoryName, userId);
            if (categoryNameExist)
                throw new ValidationException("Bu kategori adı zaten mevcut.");
            var category = new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = request.CategoryName,
                UserId = userId
            };

            var createdCategory = await _categoryRepository.AddAsync(category);
            return new CategoryDto
            {
                Id = createdCategory.Id,
                CategoryName = createdCategory.CategoryName
            };
        }
        public async Task<IReadOnlyList<CategoryDto>> GetCategoriesByUserIdAsync(Guid userId)
        {
            var categories = await _categoryRepository.GetCategoriesByUserIdAsync(userId);
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                CategoryName = c.CategoryName
            }).ToList();
        }

        public async Task UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequest request, Guid userId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null || category.UserId != userId)
                throw new NotFoundException("Kategori bulunamadı.");
            if (category.UserId != userId)
                throw new UnauthorizedException("Bu kategoriye erişim yetkiniz yok.");
            if (string.IsNullOrWhiteSpace(request.CategoryName))
                throw new ValidationException("Kategori adı boş olamaz.");

            category.CategoryName = request.CategoryName;
            bool categoryNameExist = await _categoryRepository.IsCategoryNameExistsAsync(request.CategoryName, userId);
            if (categoryNameExist)
                throw new ValidationException("Bu kategori adı zaten mevcut.");
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(Guid categoryId, Guid userId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null || category.UserId != userId)
                throw new NotFoundException("Kategori bulunamadı.");
            if (category.UserId != userId)
                throw new UnauthorizedException("Bu kategoriye erişim yetkiniz yok.");

            await _categoryRepository.DeleteAsync(category);

        }
    }
}
