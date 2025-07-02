using System;
using JobScout.Domain.SeedWork;

namespace JobScout.Application.Configuration.DomainEvents;

public class DomainEventNotificationBase<T>(T domainEvent) : IDomainEventNotification<T> where T : IDomainEvent
{
    public T DomainEvent { get; } = domainEvent;

    public Guid Id { get; } = Guid.NewGuid();
}
