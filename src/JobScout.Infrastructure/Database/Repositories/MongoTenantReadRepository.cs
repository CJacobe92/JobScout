using JobScout.Domain.Contracts;
using JobScout.Domain.Models;
using JobScout.Infrastructure.Mongo;
using MongoDB.Driver;

namespace JobScout.Infrastructure.Database.Repositories;

public class MongoTenantReadRepository(MongoDbContext mongo) : ITenantReadRepository
{
    public async Task<IEnumerable<TenantModel>> GetAllAsync(CancellationToken ct)
    {
        var tenants = await mongo.Tenants.Find(_ => true).ToListAsync(ct);

        return tenants.Select(t =>
            new TenantModel(t.Id, t.CompanyName, t.ShardKey, t.CreatedAt, t.UpdatedAt)
        );
    }
}
