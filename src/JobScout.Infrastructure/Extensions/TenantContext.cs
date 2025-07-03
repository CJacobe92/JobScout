using System;
using JobScout.Domain.Tenants;
using JobScout.Domain.Tenants.Contracts;

namespace JobScout.Infrastructure.Extensions;

public class TenantContext : ITenantContext
{
    public string Slug { get; }
    public TenantId TenantId { get; }

    public TenantContext(string slug, TenantId tenantId)
    {
        Slug = slug;
        TenantId = tenantId;
    }
}
