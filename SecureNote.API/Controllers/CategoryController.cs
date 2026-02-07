using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureNote.Application.DTOs;
using SecureNote.Application.Interfaces;
using System.Security.Claims;

namespace SecureNote.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]


    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                throw new SecureNote.Application.Exceptions.UnauthorizedException(
                    "Token geçersiz: Kullanıcı ID bulunamadı.");
            }

            return Guid.Parse(userIdClaim.Value);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyCategories()
        {
            var userId = GetUserIdFromToken();
            var categories = await _categoryService.GetCategoriesByUserIdAsync(userId);
            return Ok(categories);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var userId = GetUserIdFromToken();
            var createdCategory = await _categoryService.CreateCategoryAsync(request, userId);
            return CreatedAtAction(nameof(GetMyCategories), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            var userId = GetUserIdFromToken();
            await _categoryService.UpdateCategoryAsync(id, request, userId);
            return Ok(new { message = "Kategori başarıyla güncellendi." });
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var userId = GetUserIdFromToken();
            await _categoryService.DeleteCategoryAsync(id, userId);
            return Ok(new { message = "Kategori başarıyla silindi." });
        }
    }
}
