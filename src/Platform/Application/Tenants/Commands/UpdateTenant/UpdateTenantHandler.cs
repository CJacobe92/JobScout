using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Shared.SeedWork;

namespace Application.Tenants.Commands.UpdateTenant;

public class UpdateTenantHandler(
    ITenantRepository repository,
    ICacheService cacheService
) : IRequestHandler<UpdateTenantCommand, Tenant>
{
    private readonly ITenantRepository _repo = repository;
    private readonly ICacheService _cache = cacheService;

    public async Task<Tenant> Handle(UpdateTenantCommand command, CancellationToken ct)
    {
        var tenant = await _repo.UpdateAsync(
            command.Id,
            command.Name,
            command.License,
            command.Phone,
            command.RegisteredTo,
            command.TIN,
            command.Address,
            ct
        );

        // 🔄 Invalidate cached tenant view
        await _cache.RemoveAsync($"tenant:view:{command.Id}", ct);

        return tenant;
    }
}
