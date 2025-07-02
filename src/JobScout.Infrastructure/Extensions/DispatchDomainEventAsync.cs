using System;
using JobScout.Application.Configuration.DomainEvents;
using JobScout.Domain.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobScout.Infrastructure.Extensions;

public static class DispatchExtensions
{
    public static async Task DomainEventsAsync(this DbContext context, IMediator mediator, CancellationToken ct)
    {
        var domainEntities = context.ChangeTracker
            .Entries()
            .Where(e => e.Entity is Entity<object> entity && (entity.DomainEvents?.Count > 0))
            .Select(e => (Entity<object>)e.Entity)
            .ToList();

        foreach (var entity in domainEntities)
        {
            foreach (var domainEvent in entity.DomainEvents!)
            {
                var notificationType = typeof(DomainEventNotificationBase<>).MakeGenericType(domainEvent.GetType());
                var notification = Activator.CreateInstance(notificationType, domainEvent);
                await mediator.Publish((INotification)notification!, ct);
            }

            entity.ClearDomainEvents();
        }
    }

}
