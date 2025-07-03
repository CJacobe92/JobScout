using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobScout.Infrastructure.Registrations;

public static class InfrastructureModule
{
    public static void Register(
        IServiceCollection services,
        string appDbConnStr,
        string mongoDbConnStr
        )
    {
        // Domain Event Dispatcher
        ExtensionsModule.Register(services);

        //Contexts
        DatabaseModule.Register(services, appDbConnStr, mongoDbConnStr);

        // Rules
        TenantRulesModule.Register(services);
    }
}
