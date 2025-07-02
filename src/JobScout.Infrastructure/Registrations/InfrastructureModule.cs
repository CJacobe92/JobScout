using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobScout.Infrastructure.Registrations;

public static class InfrastructureModule
{
    public static void Register(
        IServiceCollection services,
        string appDbConnStr,
        string tenantDbConnStr)
    {
        //Contexts
        DbContextModule.Register(services, appDbConnStr, tenantDbConnStr);

        // Repository
        RepositoryModule.Register(services);

        // Identity
        IdentityModule.Register(services);

        // Rules
        TenantRulesModule.Register(services);
    }
}
