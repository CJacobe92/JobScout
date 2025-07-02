using System;
using JobScout.API.GraphQL.Tenants;
using JobScout.Application.Configuration;
using JobScout.Application.Tenants.EventHandlers;
using MediatR;

namespace JobScout.API.Configuration;

public static class APIModule
{
    public static IServiceCollection Register(IServiceCollection services)
    {
        services.AddGraphQLServer()
        .AddMutationType<TenantMutations>()
        .AddQueryType<TenantQueries>()
        .ModifyRequestOptions(o => o.IncludeExceptionDetails = true);

        services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(_ForLoadAPIAssembly).Assembly,
                    typeof(_ForLoadApplicationAssembly).Assembly,
                    typeof(TenantCreatedEventHandler).Assembly
                );
            });

        return services;
    }
}
