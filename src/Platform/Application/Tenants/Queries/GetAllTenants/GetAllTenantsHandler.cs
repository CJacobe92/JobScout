using System.Linq;
using Application.Tenants.ViewModels;
using Domain.Repositories;
using Shared.Exceptions;
using Shared.SeedWork;
using Microsoft.Extensions.Logging;

namespace Application.Tenants.Queries.GetAllTenants;

public class GetAllTenantsHandler(
    ITenantRepository tenantRepository,
    ICacheService cacheService,
    ILogger<GetAllTenantsHandler> logger
)
{
    private readonly ITenantRepository _repo = tenantRepository;
    private readonly ICacheService _cache = cacheService;
    private readonly ILogger<GetAllTenantsHandler> _logger = logger;

    public async Task<IEnumerable<TenantViewModel>> HandleAsync(GetAllTenantsQuery query, CancellationToken ct)
    {
        const int MaxPageSize = 100;

        if (query.Page < 1 || query.PageSize < 1)
            throw new BadRequestException("Page and pageSize must be greater than 0.");

        if (!string.IsNullOrWhiteSpace(query.Search) && string.IsNullOrWhiteSpace(query.By))
            throw new BadRequestException("Field-specific search requires a 'by' parameter.");

        var pageSize = query.PageSize > MaxPageSize ? MaxPageSize : query.PageSize;
        var cacheKey = $"tenants:{query.Search ?? "all"}:by:{query.By ?? "any"}:page:{query.Page}:size:{pageSize}";

        _logger?.LogInformation("Checking cache with key: {CacheKey}", cacheKey);
        var cached = await _cache.GetAsync<IEnumerable<TenantViewModel>>(cacheKey, ct);
        if (cached is not null)
        {
            _logger?.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
            return cached;
        }

        _logger?.LogInformation("Cache miss. Querying repository with key: {CacheKey}", cacheKey);
        var result = await _repo.GetAllAsync(query.Search, query.By, query.Page, pageSize, ct);

        var tenants = result.Select(t => new TenantViewModel
        {
            Id = t.Id,
            Name = t.Name,
            License = t.License,
            Phone = t.Phone,
            RegisteredTo = t.RegisteredTo,
            TIN = t.TIN,
            Address = t.Address,
            IsActivated = t.IsActivated,
            IsDeleted = t.IsDeleted,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            DeletedAt = t.DeletedAt
        }).ToList();

        await _cache.SetAsync(cacheKey, tenants, TimeSpan.FromMinutes(5), ct);
        _logger?.LogInformation("Set cache for key: {CacheKey}", cacheKey);

        return tenants;
    }
}
