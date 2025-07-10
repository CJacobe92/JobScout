using System;
using Domain.Entities;

namespace Domain.Repositories;

public interface ITenantRepository
{
    Task<Tenant> CreateAsync(
        string Name,
        string License,
        string Phone,
        string RegisteredTo,
        string TIN,
        string Address,
        CancellationToken cancellationToken
    );

    Task<IEnumerable<Tenant>> GetAllAsync(
        string? search = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    );


}
