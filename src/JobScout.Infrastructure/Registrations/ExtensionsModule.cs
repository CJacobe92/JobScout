using System;
using JobScout.Domain.SeedWork;
using JobScout.Domain.Tenants.Contracts;
using JobScout.Infrastructure.Extensions;
using JobScout.Infrastructure.Repository.Tenants;
using Microsoft.Extensions.DependencyInjection;

namespace JobScout.Infrastructure.Registrations;

public class ExtensionsModule
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
        services.AddHostedService<OutboxDispatcher>();
        services.AddScoped<ISlugResolver, SlugResolver>();

    }
}
