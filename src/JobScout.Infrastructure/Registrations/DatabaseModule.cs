using System;
using JobScout.Domain.Tenants.Contracts;
using JobScout.Infrastructure.Database;
using JobScout.Infrastructure.Identity;
using JobScout.Infrastructure.Repository.Tenants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JobScout.Infrastructure.Registrations;

public class DatabaseModule
{
    public static void Register(
        IServiceCollection services,
        string appDbConnStr,
        string mongoDbConnStr
    )
    {
        services.AddDbContext<AppDbContext>((opt) =>
        {
            opt.UseNpgsql(appDbConnStr);
        });

        services.AddScoped<TenantProvisioner>();
        services.AddScoped<TenantIdentityScopeFactory>();
        services.AddScoped<ITenantWriteRepository, TenantWriteRepository>();

        services
           .AddIdentity<AppUser, IdentityRole<Guid>>()
           .AddEntityFrameworkStores<AppDbContext>();
        services
            .AddIdentity<TenantUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<TenantDbContext>();

        // ✅ Register MongoClient using direct connection string
        services.AddSingleton<MongoClient>(new MongoClient(mongoDbConnStr));
    }
}
