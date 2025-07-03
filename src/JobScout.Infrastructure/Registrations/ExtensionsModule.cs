using System;
using JobScout.Domain.SeedWork;
using JobScout.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace JobScout.Infrastructure.Registrations;

public class ExtensionsModule
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
        services.AddHostedService<OutboxDispatcher>();
    }
}
