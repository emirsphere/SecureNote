namespace SecureNote.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<Note>? Notes { get; set; }
        public ICollection<Category>? Categories { get; set; }
    }
}
