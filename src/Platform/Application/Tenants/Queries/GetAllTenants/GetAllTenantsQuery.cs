using System;

namespace Application.Tenants.Queries.GetaLLTenants;

public record GetAllTenantsQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null);

