using MongoDB.Driver;

using Microsoft.Extensions.Options;

namespace JobScout.Infrastructure.Database;

public class MongoDbContext
{
    public readonly IMongoDatabase _db;

    public MongoDbContext(IOptions<TenantMongoDatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _db = client.GetDatabase(options.Value.DatabaseName);
    }
}