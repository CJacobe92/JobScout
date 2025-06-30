using JobScout.API.GraphQL.Inputs.Tenant;
using JobScout.Core.Queries.Tenant;
using JobScout.Core.ViewModels;
using MediatR;

namespace JobScout.API.GraphQL.Queries.Tenant;

public class TenantQueries
{
    public async Task<IEnumerable<TenantViewModel>> GetTenants(
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        return await mediator.Send(new GetAllTenantsQuery(), ct);
    }

    public async Task<TenantViewModel> GetTenantById(
    GetTenantByIdInput input,
    [Service] IMediator mediator,
    CancellationToken ct)
    {
        var query = new GetTenantByIdQuery
        {
            Id = input.Id
        };
        return await mediator.Send(query, ct);
    }
}
