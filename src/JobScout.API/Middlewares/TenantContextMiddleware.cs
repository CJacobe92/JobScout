using System;
using JobScout.Domain.Tenants;
using JobScout.Domain.Tenants.Contracts;
using JobScout.Infrastructure.Extensions;

namespace JobScout.API.Middlewares;

public class TenantContextMiddleware
{
    private readonly RequestDelegate _next;

    public TenantContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var tenantIdHeader = context.Request.Headers["x-tenant-id"].FirstOrDefault();
        var slug = context.Request.Headers["x-tenant-key"].FirstOrDefault();

        if (!string.IsNullOrEmpty(tenantIdHeader) && TenantId.TryParse(tenantIdHeader, out var parsedTenantId))
        {
            var tenantContext = new TenantContext(slug, parsedTenantId);
            context.Items[nameof(ITenantContext)] = tenantContext;
        }

        await _next(context);
    }

}