using System;
using JobScout.Domain.Tenants.Contracts;
using Microsoft.Extensions.Configuration;

namespace JobScout.Infrastructure.Database;

public class TenantPostgresDbFactory : ITenantPostgresDbFactory
{
    private readonly ITenantContext _tenant;
    private readonly string _connectionStringTemplate;

    public TenantPostgresDbFactory(ITenantContext tenantContext, IConfiguration config)
    {
        _tenant = tenantContext;
        _connectionStringTemplate = config.GetSection("ConnectionString")["AppDb"]!;
        // Example: "Host=127.0.0.1;Database=tenant_{slug};Username=postgres;Password=secret"
    }

    public string GetConnectionString()
    {
        var slugDbName = $"tenant_{_tenant.Slug}";
        return _connectionStringTemplate.Replace("{slug}", slugDbName);
    }
}
