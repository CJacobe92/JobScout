using JobScout.Core.Events;
using JobScout.Core.ViewModels;
using JobScout.Domain.Enumerations;
using JobScout.Infrastructure.Mongo;
using MediatR;
using MongoDB.Driver;

namespace JobScout.Infrastructure.Projections;

public class TenantCreatedProjectionHandler : INotificationHandler<TenantCreatedEvent>
{
    private readonly MongoDbContext _mongo;

    public TenantCreatedProjectionHandler(MongoDbContext mongo)
    {
        _mongo = mongo;
    }

    public async Task Handle(TenantCreatedEvent e, CancellationToken ct)
    {

        var projection = new TenantReadModel
        {
            Id = e.Tenant.Id,
            CompanyName = e.Tenant.CompanyName,
            ShardKey = e.Tenant.ShardKey,
            CreatedAt = e.Tenant.CreatedAt,
            UpdatedAt = e.Tenant.UpdatedAt,
        };

        await _mongo.Tenants.InsertOneAsync(projection, cancellationToken: ct);

        Console.WriteLine("✅ Projection write complete.");
    }
}
