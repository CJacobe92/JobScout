using JobScout.Infrastructure.Database;
using JobScout.Infrastructure.Identity;
using JobScout.Infrastructure.Projections; // Projection types
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace JobScout.Infrastructure.Repository.Tenants;

public class TenantProvisioner(
    TenantIdentityScopeFactory tenantIdentityScopeFactory,
    MongoClient mongoClient,
    IConfiguration configuration
)
{
    private readonly TenantIdentityScopeFactory _scopeFactory = tenantIdentityScopeFactory;
    private readonly MongoClient _mongo = mongoClient;
    private readonly IConfiguration _config = configuration;

    public async Task EnsureSchemaCreatedAsync(
        TenantUser user,
        string password,
        string tenantId,
        string slug,
        CancellationToken ct)
    {
        // 🔹 PostgreSQL provisioning
        string dbName = "tenant_" + slug;
        string defaultConnectionString = _config.GetSection("ConnectionStrings")["AppDb"];
        string newConnectionString = defaultConnectionString.Replace("jobscout_dev", dbName);

        var scoped = _scopeFactory.CreateScopedIdentityServices(newConnectionString);

        var dbContext = scoped.GetRequiredService<TenantDbContext>();
        await dbContext.Database.MigrateAsync(ct);

        var userManager = scoped.GetRequiredService<UserManager<TenantUser>>();
        await userManager.CreateAsync(user, password);

        // 🔹 MongoDB provisioning (read-side)
        var mongoDbName = $"tenant_{slug}";
        var mongoDb = _mongo.GetDatabase(mongoDbName);

        var userCollection = mongoDb.GetCollection<UserProjection>("user_profiles");

        var projection = new UserProjection
        {
            Id = user.Id.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            TenantId = tenantId,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        await userCollection.InsertOneAsync(projection, cancellationToken: ct);
    }
}
