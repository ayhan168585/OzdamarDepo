using System;

namespace OzdamarDepo.Domain.Abstractions
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.CreateVersion7();
           
        }

        public Guid Id { get; set; }

        #region Audit Log
        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreateUserId { get; set; } = default!;
        public DateTimeOffset? UpdatedAt { get; set; }
        public Guid? UpdateUserId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeleteUserId { get; set; }
        public DateTimeOffset DeletedAt { get; set; }

        #endregion
    }
}
 