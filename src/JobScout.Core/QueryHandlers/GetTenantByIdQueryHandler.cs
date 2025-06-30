using System;
using JobScout.Core.Exceptions;
using JobScout.Core.Queries.Tenant;
using JobScout.Core.ViewModels;
using JobScout.Domain.Contracts;
using JobScout.Core.Utilities;
using MediatR;

namespace JobScout.Core.QueryHandlers;

public class GetTenantByIdQueryHandler(ITenantRepository repo)
    : IRequestHandler<GetTenantByIdQuery, TenantViewModel>
{
    public async Task<TenantViewModel> Handle(GetTenantByIdQuery query, CancellationToken ct)
    {
        var tenant = await repo.GetOneById(query.Id, ct)
            ?? throw new NotFoundException($"Tenant with ID: {query.Id} not found");

        return tenant.ToViewModel();
    }
}
