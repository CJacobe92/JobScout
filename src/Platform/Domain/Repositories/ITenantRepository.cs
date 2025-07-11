using System;
using Domain.Entities;
using Shared.SeedWork;

namespace Domain.Repositories;

public interface ITenantRepository
{
    Task<Tenant> CreateAsync(
        string name,
        string license,
        string phone,
        string registeredTo,
        string tin,
        string address,
        CancellationToken cancellationToken
    );

    Task<Tenant> UpdateAsync(
        Guid id,
        string? name = null,
        string? license = null,
        string? phone = null,
        string? registeredTo = null,
        string? tin = null,
        string? address = null,
        CancellationToken ct = default
    );

    Task<PaginatedResult<Tenant>> GetAllAsync(
        string? search = null,
        string? by = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    );


}
