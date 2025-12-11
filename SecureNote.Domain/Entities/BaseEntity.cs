using System;

namespace SecureNote.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
        }


    }
}
