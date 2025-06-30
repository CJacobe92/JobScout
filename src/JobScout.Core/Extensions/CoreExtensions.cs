using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace JobScout.Core.Extensions;

public static class CoreExtensions
{
  public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration config)
  {

    services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssemblies(typeof(_ForCoreAssemblyLoadOnly).Assembly);
    });

    return services;
  }
}
