using System;
using JobScout.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace JobScout.Infrastructure.Extensions;

public static class DomainEventDispatcherExtensions
{
    public static async Task DispatchTrackedDomainEventsAsync(
        this DbContext context,
        IDomainEventDispatcher dispatcher,
        CancellationToken ct)
    {
        var entitiesWithEvents = context.ChangeTracker
            .Entries<EntityBase>()
            .Where(e => e.Entity.DomainEvents is { Count: > 0 })
            .Select(e => e.Entity);

        foreach (var entity in entitiesWithEvents)
        {
            Console.WriteLine($"📢 Dispatching events from: {entity.GetType().Name}");

            foreach (var domainEvent in entity.DomainEvents!)
            {
                Console.WriteLine($"↪ Event: {domainEvent.GetType().Name}");
            }

            await dispatcher.DispatchFromEntityAsync(entity, ct);
        }
    }
}
