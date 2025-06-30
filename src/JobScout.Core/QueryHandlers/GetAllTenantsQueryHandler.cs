using JobScout.Core.Queries.Tenant;
using JobScout.Core.ViewModels;
using JobScout.Domain.Contracts;
using MediatR;

namespace JobScout.Core.QueryHandlers;

public class GetAllTenantsQueryHandler(ITenantReadRepository repo)
    : IRequestHandler<GetAllTenantsQuery, IEnumerable<TenantViewModel>>
{
    public async Task<IEnumerable<TenantViewModel>> Handle(GetAllTenantsQuery query, CancellationToken ct)
    {
        var tenants = await repo.GetAllAsync(ct);

        return tenants.Select(t =>
             new TenantViewModel(t.Id, t.CompanyName, t.CreatedAt, t.UpdatedAt)
        );
    }
}
