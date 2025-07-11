using System;

namespace Shared.SeedWork;

public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents() => _domainEvents?.Clear();

    public IEnumerable<IDomainEvent> PullDomainEvents()
    {
        var events = _domainEvents?.ToList() ?? [];
        ClearDomainEvents();
        return events;
    }
}
