using System;

namespace Everest.Engineering.Data.Models
{
    public class DbEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public DbEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            LastModifiedAt = CreatedAt;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
