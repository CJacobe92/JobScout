using System;
using JobScout.Application.Configuration.DomainEvents;
using JobScout.Domain.SeedWork;
using MediatR;

namespace JobScout.Infrastructure.Extensions;

public class MediatRDomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct)
    {
        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventNotificationBase<>).MakeGenericType(domainEvent.GetType());
            var notification = Activator.CreateInstance(notificationType, domainEvent);
            await mediator.Publish((INotification)notification!, ct);
        }
    }

    public async Task DispatchFromEntityAsync(EntityBase entity, CancellationToken ct)
    {
        var domainEvents = entity.PullDomainEvents();
        await DispatchAsync(domainEvents, ct);
    }
}

