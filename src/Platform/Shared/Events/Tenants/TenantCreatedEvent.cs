using System;
using Shared.SeedWork;

namespace Shared.Events.Tenants;

public class TenantCreatedEvent(Guid Id, string Name, string License) : DomainEvent
{
    public Guid Id = Id;
    public string Name = Name;
    public string License = License;
}
