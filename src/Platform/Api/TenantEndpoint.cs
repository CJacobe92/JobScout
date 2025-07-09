using System;
using Application.Commands;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Events;
using StackExchange.Redis;
using System.Text.Json;
using Api.Dtos;

namespace Api;

public static class TenantEndpoints
{
    public static void MapTenantEndpoints(this WebApplication app)
    {
        app.MapPost("/api/tenants", async (
            CreateTenantCommand command,
            AppDbContext db,
            IConnectionMultiplexer redis,
            IConfiguration config,
            CancellationToken ct) =>
        {
            var tenant = new Tenant(
                command.Name,
                command.License,
                command.Phone,
                command.RegisteredTo,
                command.TIN,
                command.Address
            );

            db.Tenants.Add(tenant);

            var evt = new TenantCreatedEvent(tenant.Id, tenant.Name, tenant.License);
            db.AddOutboxEvent(evt);

            await db.SaveChangesAsync(ct);

            var redisHost = config["Redis:Host"] ?? "redis"; // fallback to "redis" if not set
            var redisPort = config["Redis:Port"] ?? "6379";

            var server = redis.GetServer($"{redisHost}:{redisPort}");
            var keys = server.Keys(pattern: "tenants:*").ToArray();

            foreach (var key in keys)
            {
                await redis.GetDatabase().KeyDeleteAsync(key);
            }

            return Results.Created($"/api/tenants/{tenant.Id}", tenant);
        });


        app.MapGet("/api/tenants", async (
            AppDbContext db,
            IConnectionMultiplexer redis,
            CancellationToken ct,
            string? search,
            int page = 1,
            int pageSize = 10) =>
        {
            const int MaxPageSize = 100;

            if (page < 1 || pageSize < 1)
                return Results.BadRequest("Page and pageSize must be greater than 0.");

            if (pageSize > MaxPageSize)
                pageSize = MaxPageSize;

            var cacheKey = $"tenants:{search}:{page}:{pageSize}";
            var cache = redis.GetDatabase();

            // Try to get from cache
            var cached = await cache.StringGetAsync(cacheKey);
            if (cached.HasValue)
            {
                var cachedResult = JsonSerializer.Deserialize<TenantListDto>(cached!);
                return Results.Ok(cachedResult);
            }

            // Query from DB
            var query = db.Tenants
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t =>
                    EF.Functions.ILike(t.Name, $"%{search}%") ||
                    EF.Functions.ILike(t.License, $"%{search}%"));
            }

            var totalCount = await query.CountAsync(ct);

            var tenants = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TenantDto(
                    t.Id,
                    t.Name,
                    t.License
                ))
                .ToListAsync(ct);

            var evt = new TenantsFetchedEvent(tenants.Count, DateTime.UtcNow);
            db.AddOutboxEvent(evt);
            await db.SaveChangesAsync(ct);

            var result = new TenantListDto
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = tenants
            };

            // Cache the result for 30 seconds
            await cache.StringSetAsync(
                cacheKey,
                JsonSerializer.Serialize(result),
                TimeSpan.FromSeconds(30));

            return Results.Ok(result);
        });

    }
}