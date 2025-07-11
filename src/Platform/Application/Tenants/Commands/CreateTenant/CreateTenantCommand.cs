using System;

namespace Application.Tenants.Commands.CreateTenant;

public record CreateTenantCommand(
    string Name,
    string License,
    string Phone,
    string RegisteredTo,
    string TIN,
    string Address
);
