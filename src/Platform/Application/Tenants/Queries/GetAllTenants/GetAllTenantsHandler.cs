using System;
using Domain.Entities;
using Domain.Repositories;
using Shared.Exceptions;

namespace Application.Tenants.Queries.GetaLLTenants;

public class GetAllTenantsHandler(ITenantRepository tenantRepository)
{
    private readonly ITenantRepository _repo = tenantRepository;

    public async Task<IEnumerable<Tenant>> HandleAsync(GetAllTenantsQuery query, CancellationToken ct)
    {
        const int MaxPageSize = 100;

        if (query.Page < 1 || query.PageSize < 1)
            throw new BadRequestException("Page and pageSize must be greater than 0.");

        var pageSize = query.PageSize > MaxPageSize ? MaxPageSize : query.PageSize;

        return await _repo.GetAllAsync(query.Search, query.Page, query.PageSize, ct);
    }
}
