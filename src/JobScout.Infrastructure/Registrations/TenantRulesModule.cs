using System;
using JobScout.Domain.SeedWork;
using JobScout.Infrastructure.Repository.Tenants;
using Microsoft.Extensions.DependencyInjection;

namespace JobScout.Infrastructure.Registrations;

public static class TenantRulesModule
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IUniquenessChecker<string>, CompanyUniquenessChecker>();
    }
}
