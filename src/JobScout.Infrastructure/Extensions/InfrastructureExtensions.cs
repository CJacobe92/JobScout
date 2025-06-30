using System;
using JobScout.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using JobScout.Infrastructure.Database.Entities;
using JobScout.Infrastructure.Database.Repositories;
using JobScout.Domain.Contracts;

namespace JobScout.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
  {
    var useInMemory = bool.TryParse(config["UseInMemoryDatabase"], out var val) && val;

    if (useInMemory)
    {
      services.AddDbContext<CoreDbContext>(opt =>
        opt.UseInMemoryDatabase("CoreDb")
      );

      services.AddDbContext<ShardDbContext>(opt =>
        opt.UseInMemoryDatabase("ShardDb")
      );
    }

    else
    {
      var defaultConnStr = config.GetConnectionString("DefaultConnection");
      var shard00ConnStr = config.GetSection("ShardConnections")["shard00"];


      services.AddDbContext<CoreDbContext>((sp, opt) =>
        {
          opt.UseNpgsql(defaultConnStr);
        }
      );

      services.AddDbContext<ShardDbContext>(opt =>
        {
          opt.UseNpgsql(shard00ConnStr);
        }
      );
    }

    services.AddIdentity<TenantUserEntity, IdentityRole<Guid>>()
      .AddEntityFrameworkStores<ShardDbContext>();

    services.AddScoped<DbContext>(sp => sp.GetRequiredService<CoreDbContext>());
    services.AddScoped<ITenantRepository, TenantRepository>();

    return services;
  }

}
