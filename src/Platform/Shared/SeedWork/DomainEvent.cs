using System;

namespace Shared.SeedWork;

public class DomainEvent : IDomainEvent
{
    public DateTime OccuredOn { get; }
    public DomainEvent()
    {
        this.OccuredOn = DateTime.UtcNow;
    }
}
