using System;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Shared.SeedWork;

namespace Application.Tenants.Commands.CreateTenant;

public class CreateTenantHandler(
    ITenantRepository tenantRepository,
    ICacheService cacheService
) : IRequestHandler<CreateTenantCommand, Tenant>
{
    private readonly ITenantRepository _repo = tenantRepository;
    private readonly ICacheService _cache = cacheService;

    public async Task<Tenant> Handle(CreateTenantCommand command, CancellationToken ct)
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

        await _cache.RemoveByPatternAsync("tenants:*", ct);

        return tenant;
    }
}
