using System;

namespace JobScout.Domain.Tenants.Contracts;

public interface ITenantPostgresDbFactory
{
    string GetConnectionString();
}
