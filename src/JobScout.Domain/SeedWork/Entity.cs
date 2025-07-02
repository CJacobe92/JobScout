using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.SeedWork
{
    public abstract class Entity<TId>
    {

        public TId? Id { get; init; }

        private List<IDomainEvent>? _domainEvents;

        public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents = _domainEvents ?? [];
            this._domainEvents.Add(domainEvent);

        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
