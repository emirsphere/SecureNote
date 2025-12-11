namespace SecureNote.Domain.Entities
{
    public class Note : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public Guid UserId { get; set; } 
        public User? User { get; set; }
    }
}