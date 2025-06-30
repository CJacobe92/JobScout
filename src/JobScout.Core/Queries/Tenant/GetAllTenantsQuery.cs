using System;
using JobScout.Core.ViewModels;
using MediatR;

namespace JobScout.Core.Queries.Tenant;

public class GetAllTenantsQuery : IRequest<IEnumerable<TenantViewModel>>
{
    // public GetAllTenantsQuery() { }
}
