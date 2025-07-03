using System;

namespace JobScout.Domain.Tenants.Contracts;

public interface ITenantContext
{
    string Slug { get; }
    TenantId TenantId { get; }
}
