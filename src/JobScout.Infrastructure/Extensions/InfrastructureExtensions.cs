using System;
using JobScout.Domain.Contracts;
using JobScout.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using JobScout.Infrastructure.Database.Entities;
using JobScout.Infrastructure.Database.Repositories;


namespace JobScout.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
  {
    var defaultConnStr = config.GetConnectionString("DefaultConnection");
    var shard00ConnStr = config.GetSection("ShardConnections")["shard00"];

    // DB Contexts
    services.AddDbContext<CoreDbContext>(options => options.UseNpgsql(defaultConnStr));
    services.AddDbContext<ShardDbContext>(options => options.UseNpgsql(shard00ConnStr));

    // Shared DbContext registration
    services.AddScoped<DbContext>(sp => sp.GetRequiredService<CoreDbContext>());

    // UoW and Identity
    services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
    services.AddIdentity<TenantUserEntity, IdentityRole<Guid>>() // <– use the EF Identity entity
    .AddEntityFrameworkStores<ShardDbContext>();

    services.AddScoped<ITenantRepository, TenantRepository>();

    return services;
  }
}
