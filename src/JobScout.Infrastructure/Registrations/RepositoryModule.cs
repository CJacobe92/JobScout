using System;
using JobScout.Domain.Tenants.Contracts;
using JobScout.Infrastructure.Repository.Tenants;
using Microsoft.Extensions.DependencyInjection;

namespace JobScout.Infrastructure.Registrations;

public static class RepositoryModule
{
    public static void Register(IServiceCollection services)
    {
        // 
        services.AddScoped<ITenantWriteRepository, TenantWriteRepository>();
        services.AddScoped<IShardResolver, ShardResolver>();
    }
}
