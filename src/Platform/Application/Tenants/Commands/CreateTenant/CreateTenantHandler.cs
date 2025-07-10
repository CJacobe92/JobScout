using System;
using Application.Commands.CreateTenant;
using Domain.Entities;
using Domain.Repositories;
using Shared.Events.Tenants;

namespace Application.Tenants.Commands.CreateTenant;

public class CreateTenantHandler(
    ITenantRepository tenantRepository
)
{
    private readonly ITenantRepository _repo = tenantRepository;

    public async Task<Tenant> HandleAsync(CreateTenantCommand command, CancellationToken ct)
    {
        var tenant = await _repo.CreateAsync(
            command.Name,
            command.License,
            command.Phone,
            command.RegisteredTo,
            command.TIN,
            command.Address,
            ct
        );

        return tenant;
    }
}
