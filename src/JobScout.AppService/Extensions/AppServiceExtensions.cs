using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JobScout.AppService;

namespace JobScout.App.Extensions;

public static class AppServiceExtensions
{
  public static IServiceCollection AddAppExtensions(this IServiceCollection services, IConfiguration config)
  {
    services.AddAutoMapper(typeof(_ForAppServiceAssemblyLoadOnly).Assembly);
    return services;
  }
}
