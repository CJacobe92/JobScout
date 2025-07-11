using System;
using Application.Tenants.ViewModels;
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
        string? by = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default)
    {
        var dbQuery = _context.Tenants.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            dbQuery = by?.ToLowerInvariant() switch
            {
                "name" => dbQuery.Where(t =>
                    EF.Functions.ILike(t.Name, $"%{search}%")),
                "registeredto" => dbQuery.Where(t =>
                    EF.Functions.ILike(t.RegisteredTo, $"%{search}%")),
                "license" => dbQuery.Where(t =>
                    EF.Functions.ILike(t.License, $"%{search}%")),
                _ => dbQuery.Where(t => // fallback: search across all three
                    EF.Functions.ILike(t.Name, $"%{search}%") ||
                    EF.Functions.ILike(t.RegisteredTo, $"%{search}%") ||
                    EF.Functions.ILike(t.License, $"%{search}%"))
            };
        }

        dbQuery = dbQuery.OrderBy(t => t.Name);

        var tenants = await dbQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return tenants;
    }
}
