using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureNote.Application.DTOs;
using SecureNote.Application.Interfaces;
using System.Security.Claims;

namespace SecureNote.API.Controllers
{
    [Authorize] // DİKKAT: Bu kapıdaki güvenlik görevlisidir. Token'ı olmayan giremez!
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        // GET api/note (Kullanıcının kendi notlarını getirir)
        [HttpGet("get notes")]
        public async Task<IActionResult> GetMyNotes()
        {
            try
            {
                // 1. Token'dan ID'yi Oku (Kim bu adam?)
                var userId = GetUserIdFromToken();

                // 2. Servise Git (Notları getir)
                var notes = await _noteService.GetNotesByUserIdAsync(userId);

                return Ok(notes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST api/note (Yeni not ekle)
        [HttpPost ("create post")]
        public async Task<IActionResult> CreateNote([FromBody] NoteDto request)
        {
            try
            {
                var userId = GetUserIdFromToken();

                var createdNote = await _noteService.CreateNoteAsync(request, userId);

                // 201 Created ve oluşturulan notun kendisi
                return CreatedAtAction(nameof(GetMyNotes), new { id = createdNote.Id }, createdNote);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT api/note (Not güncelle)
        [HttpPut("update note")]
        public async Task<IActionResult> UpdateNote([FromBody] UpdateNoteRequest request)
        {
            try
            {
                var userId = GetUserIdFromToken();
                await _noteService.UpdateNoteAsync(request, userId);

                return Ok(new { message = "Not başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE api/note/{id} (Not sil)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            try
            {
                var userId = GetUserIdFromToken();
                await _noteService.DeleteNoteAsync(id, userId);

                return Ok(new { message = "Not başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // --- YARDIMCI METOT ---
        // Token'ın içindeki "NameIdentifier" (bizim ID'miz) claim'ini okur.
        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                throw new Exception("Token geçersiz: Kullanıcı ID bulunamadı.");
            }

            return Guid.Parse(userIdClaim.Value);
        }
    }
}