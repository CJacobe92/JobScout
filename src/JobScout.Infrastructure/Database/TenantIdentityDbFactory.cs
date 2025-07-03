using JobScout.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JobScout.Infrastructure.Database;

public class TenantIdentityScopeFactory
{
    public IServiceProvider CreateScopedIdentityServices(string connectionString)
    {
        var services = new ServiceCollection();

        services.AddLogging();

        services.AddDbContext<TenantDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services
            .AddIdentity<TenantUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<TenantDbContext>();

        return services.BuildServiceProvider();
    }
}
