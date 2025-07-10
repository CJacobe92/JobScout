using System;

namespace Application.Tenants.Validators.CreateTenant;

public record CreateTenantDto(
    string Name,
    string License,
    string Phone,
    string RegisteredTo,
    string TIN,
    string Address
);
