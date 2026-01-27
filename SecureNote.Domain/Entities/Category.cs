
namespace SecureNote.Domain.Entities
{
    public class Category: BaseEntity
    {
        public string CategoryName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Note>? Note { get; set; }
    }
}
