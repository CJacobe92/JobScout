using JobScout.Infrastructure.Database;
using JobScout.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobScout.Infrastructure.Repository.Tenants;

public class TenantProvisioner(
    TenantIdentityScopeFactory tenantIdentityScopeFactory,
    IConfiguration configuration
)
{
    private readonly TenantIdentityScopeFactory _scopeFactory = tenantIdentityScopeFactory;
    private readonly IConfiguration _config = configuration;

    public async Task EnsureSchemaCreatedAsync(
        TenantUser user,
        string password,
        string schemaName,
        CancellationToken ct)
    {
        string dbName = "tenant_" + schemaName;
        string defaultConnectionString = _config.GetSection("ConnectionStrings")["AppDb"];
        string newConnectionString = defaultConnectionString.Replace("jobscout_dev", dbName);

        var scoped = _scopeFactory.CreateScopedIdentityServices(newConnectionString);

        var dbContext = scoped.GetRequiredService<TenantDbContext>();
        await dbContext.Database.MigrateAsync(ct);

        var userManager = scoped.GetRequiredService<UserManager<TenantUser>>();
        await userManager.CreateAsync(user, password);
    }
}
