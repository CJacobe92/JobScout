using System;
using JobScout.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobScout.Infrastructure.Registrations;

public class DbContextModule
{
    public static void Register(
        IServiceCollection services,
        string appDbConnStr,
        string tenantDbConnStr
        )
    {
        services.AddDbContext<AppDbContext>((opt) =>
        {
            opt.UseNpgsql(appDbConnStr);
        });

        services.AddDbContext<TenantDbContext>((opt) =>
        {
            opt.UseNpgsql(tenantDbConnStr);
        });
    }
}
