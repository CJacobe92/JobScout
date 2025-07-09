using System.Text.Json;
using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DbContextExtensions
{
    public static void AddOutboxEvent(this DbContext db, object @event)
    {
        var message = new OutboxMessage
        {
            Type = @event.GetType().Name,
            Payload = JsonSerializer.Serialize(@event)
        };

        db.Set<OutboxMessage>().Add(message);
    }
}
