using MongoDB.Driver;
using JobScout.Core.ViewModels;
using Microsoft.Extensions.Options;

namespace JobScout.Infrastructure.Mongo;

public class MongoDbContext
{
    private readonly IMongoDatabase _db;

    public MongoDbContext(IOptions<MongoDbSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _db = client.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<TenantReadModel> Tenants =>
        _db.GetCollection<TenantReadModel>("Tenants");
}
