using System;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TenantRepository(AppDbContext context) : ITenantRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Tenant> CreateAsync(
        string name,
        string license,
        string phone,
        string registeredTo,
        string tin,
        string address,
        CancellationToken ct)
    {
        var tenant = Tenant.Create(
            name,
            license,
            phone,
            registeredTo,
            tin,
            address
        );

        await _context.Tenants.AddAsync(tenant, ct);

        return tenant;
    }

    public async Task<IEnumerable<Tenant>> GetAllAsync(
    string? search = null,
    int page = 1,
    int pageSize = 10,
    CancellationToken ct = default)
    {
        var dbQuery = _context.Tenants
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            dbQuery = dbQuery.Where(t =>
                EF.Functions.ILike(t.Name!, $"%{search}%") ||
                EF.Functions.ILike(t.License, $"%{search}%"));
        }

        dbQuery = dbQuery.OrderBy(t => t.Name);

        var tenants = await dbQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return tenants;
    }
}
