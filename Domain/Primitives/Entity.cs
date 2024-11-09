using System;
using System.Collections.Generic;

namespace Domain.Primitives
{
    public abstract class Entity : ISoftDeletable
    {
        private readonly List<DomainEvent> _domainEvents = new();

        protected Entity()
        {
            CreatedOn = DateTime.UtcNow;
            IsActive = true;
            IsDeleted = false;
        }

        public bool IsActive { get; private set; }

        public DateTime CreatedOn { get; init; }

        public DateTime? ModifiedOn { get; private set; }

        public bool IsDeleted { get; private set; }

        public DateTime? DeletedOn { get; private set; }

        public ICollection<DomainEvent> GetDomainEvents() => _domainEvents;

        protected void SetModifiedTimestamp(DateTime modifiedAt)
        {
            ModifiedOn = modifiedAt;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        protected void Raise(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
