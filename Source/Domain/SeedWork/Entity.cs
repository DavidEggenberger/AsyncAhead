﻿using Domain.Shared.Exceptions;
using Shared.Kernel;

namespace Domain.SharedKernel
{
    public abstract class Entity : IAuditable
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public bool IsSoftDeleted { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        protected void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents.Add(eventItem);
        }
        protected void RemoveDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
        public void SoftDelete()
        {
            if (IsSoftDeleted is true)
            {
                throw new InvalidEntityDeleteException("");
            }
            else
            {
                IsSoftDeleted = true;
            }
        }
        public void UndoSoftDelete()
        {
            IsSoftDeleted = false;
        }
    }
}