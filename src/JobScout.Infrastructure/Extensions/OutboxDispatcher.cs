using JobScout.Domain.SeedWork;
using JobScout.Infrastructure.Database;
using JobScout.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobScout.Infrastructure.Extensions;

public class OutboxDispatcher : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public OutboxDispatcher(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        Console.WriteLine("🚀 OutboxDispatcher started.");

        while (!ct.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var dispatcher = scope.ServiceProvider.GetRequiredService<IDomainEventDispatcher>();

                var messages = await db.Outbox
                    .Where(m => m.ProcessedOn == null)
                    .OrderBy(m => m.CreatedOn)
                    .Take(20)
                    .ToListAsync(ct);

                Console.WriteLine($"🔎 Outbox Query Returned: {messages.Count} messages");

                foreach (var message in messages)
                {
                    Console.WriteLine("📤 Outbox Dispatch Start");
                    Console.WriteLine($"🆔 Message ID: {message.Id}");
                    Console.WriteLine($"📦 EventType: {message.EventType}");
                    Console.WriteLine($"📅 CreatedOn: {message.CreatedOn}");
                    Console.WriteLine($"📄 Payload: {message.ContentJson}");

                    var resolvedType = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .FirstOrDefault(t =>
                            string.Equals(t.FullName, message.EventType, StringComparison.OrdinalIgnoreCase));

                    if (resolvedType is null)
                    {
                        Console.WriteLine($"⚠️ Failed to resolve type: {message.EventType}");
                        continue;
                    }

                    var domainEvent = JsonHelper.Deserialize(message.ContentJson, resolvedType);

                    if (domainEvent is IDomainEvent typedEvent)
                    {
                        await dispatcher.DispatchAsync([typedEvent], ct);
                        message.ProcessedOn = DateTime.UtcNow;

                        Console.WriteLine($"✅ Dispatched: {message.EventType} → Marked ProcessedOn");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Skipped: Deserialized object is not IDomainEvent → {message.EventType}");
                    }

                    Console.WriteLine("📤 Outbox Dispatch End\n");
                }

                await db.SaveChangesAsync(ct);
                Console.WriteLine($"🕒 Waiting 30mins before next dispatch cycle at {DateTime.UtcNow:T}");
                await Task.Delay(TimeSpan.FromMinutes(30), ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 Dispatcher error: {ex.Message}");
            }
        }
    }
}
