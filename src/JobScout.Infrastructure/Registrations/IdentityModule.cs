using System;
using JobScout.Infrastructure.Database;
using JobScout.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JobScout.Infrastructure.Registrations;

public class IdentityModule
{
    public static void Register(IServiceCollection services)
    {
        services
            .AddIdentity<AppUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();

        services
            .AddIdentity<TenantUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<TenantDbContext>();
    }
}
