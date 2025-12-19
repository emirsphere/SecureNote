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
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        /// <summary>
        /// Kullanıcının kendi notlarını getirir
        /// </summary>
        /// <returns>Not listesi</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyNotes()
        {
            var userId = GetUserIdFromToken();
            var notes = await _noteService.GetNotesByUserIdAsync(userId);
            return Ok(notes);
        }

        /// <summary>
        /// Yeni not oluşturur
        /// </summary>
        /// <param name="request">Not bilgileri</param>
        /// <returns>Oluşturulan not</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNote([FromBody] NoteDto request)
        {
            var userId = GetUserIdFromToken();
            var createdNote = await _noteService.CreateNoteAsync(request, userId);
            return CreatedAtAction(nameof(GetMyNotes), new { id = createdNote.Id }, createdNote);
        }

        /// <summary>
        /// Not günceller
        /// </summary>
        /// <param name="id">Not ID'si</param>
        /// <param name="request">Güncellenen not bilgileri</param>
        /// <returns>Başarılı mesajı</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] UpdateNoteRequest request)
        {
            var userId = GetUserIdFromToken();
            await _noteService.UpdateNoteAsync(id, request, userId);
            return Ok(new { message = "Not başarıyla güncellendi." });
        }

        /// <summary>
        /// Not siler
        /// </summary>
        /// <param name="id">Not ID'si</param>
        /// <returns>Başarılı mesajı</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var userId = GetUserIdFromToken();
            await _noteService.DeleteNoteAsync(id, userId);
            return Ok(new { message = "Not başarıyla silindi." });
        }

        /// <summary>
        /// Token'dan kullanıcı ID'sini okur
        /// </summary>
        /// <returns>Kullanıcı ID'si</returns>
        /// <exception cref="UnauthorizedException">Token geçersiz ise</exception>
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
    }
}