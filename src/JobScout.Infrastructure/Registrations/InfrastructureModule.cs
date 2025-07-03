using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobScout.Infrastructure.Registrations;

public static class InfrastructureModule
{
    public static void Register(
        IServiceCollection services,
        string appDbConnStr
        )
    {
        // Domain Event Dispatcher
        ExtensionsModule.Register(services);

        //Contexts
        DatabaseModule.Register(services, appDbConnStr);

        // Rules
        TenantRulesModule.Register(services);
    }
}
