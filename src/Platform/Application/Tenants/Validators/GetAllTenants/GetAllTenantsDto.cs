using System;

namespace Application.Tenants.Validators.GetAllTenants;

public record GetAllTenantsDto(
    string? Search = null,
    string? By = null,
    int Page = 1,
    int PageSize = 10
);
