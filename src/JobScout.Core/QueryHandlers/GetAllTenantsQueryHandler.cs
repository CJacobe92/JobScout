using JobScout.Core.Queries.Tenant;
using JobScout.Core.ViewModels;
using JobScout.Domain.Contracts;
using JobScout.Core.Utilities;
using MediatR;

namespace JobScout.Core.QueryHandlers;

public class GetAllTenantsQueryHandler(
    ITenantRepository repo
) : IRequestHandler<GetAllTenantsQuery, IEnumerable<TenantViewModel>>
{
    public async Task<IEnumerable<TenantViewModel>> Handle(GetAllTenantsQuery query, CancellationToken ct)
    {
        var tenants = await repo.GetAll(ct);

        return tenants.ToViewModel();

    }
}
