using Infrastructure.Kafka;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Shared.Contracts.Events;

namespace Infrastructure.Outbox;

public class OutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<OutboxProcessor> _logger;
    private const int MaxRetries = 3;

    public OutboxProcessor(IServiceProvider provider, ILogger<OutboxProcessor> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var kafka = scope.ServiceProvider.GetRequiredService<IKafkaProducer>();

            var messages = await db.OutboxMessages
                .Where(m => m.ProcessedAt == null && m.RetryCount < MaxRetries)
                .OrderBy(m => m.CreatedAt)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    switch (msg.Type)
                    {
                        case nameof(TenantCreatedEvent):
                            await kafka.ProduceAsync(KafkaTopics.TenantCreated, msg.Payload);
                            break;

                        case nameof(TenantsFetchedEvent):
                            await kafka.ProduceAsync(KafkaTopics.TenantsFetched, msg.Payload);
                            break;

                        default:
                            _logger.LogWarning("Unknown event type: {Type}", msg.Type);
                            continue;
                    }

                    msg.ProcessedAt = DateTime.UtcNow;
                    msg.LastError = null;
                }
                catch (Exception ex)
                {
                    msg.RetryCount++;
                    msg.LastError = ex.Message;

                    _logger.LogWarning(ex, "Retry {RetryCount} failed for message {Id}", msg.RetryCount, msg.Id);

                    if (msg.RetryCount >= MaxRetries)
                    {
                        await kafka.ProduceAsync(KafkaTopics.TenantCreatedDLQ, msg.Payload);
                        db.OutboxMessages.Remove(msg);
                        _logger.LogError("Moved message {Id} to DLQ topic", msg.Id);
                    }
                }
            }


            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
