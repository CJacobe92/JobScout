using System;
using Application.Tenants.ViewModels;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.SeedWork;

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

    public async Task<Tenant> UpdateAsync(
    Guid id,
    string? name = null,
    string? license = null,
    string? phone = null,
    string? registeredTo = null,
    string? tin = null,
    string? address = null,
    CancellationToken ct = default)
    {
        var tenant = await FindAsync(id, ct)
            ?? throw new ArgumentException($"Tenant with id {id} not found");

        tenant.Update(name, license, phone, registeredTo, tin, address);

        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync(ct);

        return tenant;
    }


    public async Task<PaginatedResult<Tenant>> GetAllAsync(
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
                _ => dbQuery.Where(t =>
                    EF.Functions.ILike(t.Name, $"%{search}%") ||
                    EF.Functions.ILike(t.RegisteredTo, $"%{search}%") ||
                    EF.Functions.ILike(t.License, $"%{search}%"))
            };
        }

        dbQuery = dbQuery.OrderBy(t => t.Name);

        return await dbQuery.ToPaginatedResultAsync(page, pageSize, ct);
    }

    private async Task<Tenant> FindAsync(Guid id, CancellationToken ct)
    {
        var result = await _context.Tenants.FirstOrDefaultAsync(t => t.Id == id, ct)
            ?? throw new ArgumentException($"Tenant with id:{id} not found");
        return result;
    }
}
