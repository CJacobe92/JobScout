using JobScout.API.GraphQL.Mutations.Tenant;
using JobScout.API.GraphQL.Queries.Tenant;

namespace JobScout.API.Extensions;

public static class ApiExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
    {

        services.AddGraphQLServer()
            .AddQueryType<TenantQueries>()
            .AddMutationType<TenantMutations>()
            .ModifyRequestOptions(o => o.IncludeExceptionDetails = true);
        return services;
    }
}