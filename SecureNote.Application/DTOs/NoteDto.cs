using System;

namespace SecureNote.Application.DTOs
{
    // Not oluşturma işlemi için kullanılan sınıf
    public class NoteDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public Guid? CategoryId { get; set; }
    }

    public class UpdateNoteRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        // YENİ EKLENEN: Kategori güncellemesi
        public Guid? CategoryId { get; set; }
    }

    // Kullanıcıya notu gösterirken kullanılan sınıf
    public class ResponseNote
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }

        // YENİ EKLENEN: Notun kategori bilgisi (Kullanıcıya göstermek için)
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}