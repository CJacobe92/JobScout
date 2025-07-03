
using JobScout.Domain.Tenants.Contracts;
using JobScout.Infrastructure.Database;
using JobScout.Infrastructure.Extensions;
using JobScout.Infrastructure.Identity;
using JobScout.Infrastructure.Repository.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace JobScout.Infrastructure.Registrations;

public static class DatabaseModule
{
    public static void Register(
        IServiceCollection services,
        string appDbConnStr,
        string mongoDbConnStr)
    {
        // Global AppDbContext (shared)
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(appDbConnStr);
        });

        // Tenant-aware DbContext (per request)
        services.AddScoped<ITenantPostgresDbFactory, TenantPostgresDbFactory>();

        services.AddScoped<TenantDbContext>(sp =>
        {
            var factory = sp.GetRequiredService<ITenantPostgresDbFactory>();
            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
            optionsBuilder.UseNpgsql(factory.GetConnectionString());
            return new TenantDbContext(optionsBuilder.Options);
        });

        // Identity
        services
            .AddIdentity<AppUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();

        services
            .AddIdentity<TenantUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<TenantDbContext>();

        // Mongo
        services.AddSingleton<MongoClient>(new MongoClient(mongoDbConnStr));

        // Tenant context from middleware
        services.AddHttpContextAccessor();
        services.AddScoped<ITenantContext>(sp =>
        {
            var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
            return httpContext?.Items[nameof(ITenantContext)] as ITenantContext
                ?? throw new InvalidOperationException("Tenant context not resolved");
        });

        // Custom services
        services.AddScoped<TenantProvisioner>();
        services.AddScoped<TenantIdentityScopeFactory>();
        services.AddScoped<ITenantWriteRepository, TenantWriteRepository>();
    }
}

