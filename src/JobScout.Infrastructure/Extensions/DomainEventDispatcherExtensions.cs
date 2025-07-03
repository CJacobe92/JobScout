using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using JobScout.Domain.Outbox;
using JobScout.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using JobScout.Infrastructure.Extensions; // for JsonHelper

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

        var outboxEntries = new List<Outbox>();

        foreach (var entity in entitiesWithEvents)
        {
            Console.WriteLine($"📢 Dispatching events from: {entity.GetType().Name}");

            foreach (var domainEvent in entity.DomainEvents!)
            {
                Console.WriteLine($"↪ Event: {domainEvent.GetType().Name}");

                var concreteType = domainEvent.GetType();
                var contentJson = JsonHelper.Serialize(domainEvent, concreteType);

                Console.WriteLine($"📦 Serialized Payload: {contentJson}");

                var outboxEntry = new Outbox
                {
                    EventType = concreteType.FullName!,
                    ContentJson = contentJson,
                    CreatedOn = DateTime.UtcNow
                };

                outboxEntries.Add(outboxEntry);
            }

            await dispatcher.DispatchFromEntityAsync(entity, ct);
        }

        await context.Set<Outbox>().AddRangeAsync(outboxEntries, ct);
        await context.SaveChangesAsync(ct);
    }
}
