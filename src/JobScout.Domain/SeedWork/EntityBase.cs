using System;

namespace JobScout.Domain.SeedWork;

public abstract class EntityBase
{
    private List<IDomainEvent>? _domainEvents;

    public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= [];
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents?.Clear();

    public IEnumerable<IDomainEvent> PullDomainEvents()
    {
        var events = _domainEvents?.ToList() ?? [];
        ClearDomainEvents();
        return events;
    }
}
