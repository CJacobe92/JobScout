using System;

namespace Application.Tenants.Queries.GetAllTenants;

public record GetAllTenantsQuery(
    string? Search = null,
    string? By = null,
    int Page = 1,
    int PageSize = 10
);

