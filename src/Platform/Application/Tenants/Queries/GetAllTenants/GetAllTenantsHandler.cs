using Application.Tenants.ViewModels;
using Domain.Repositories;
using Shared.Exceptions;
using Shared.SeedWork;
using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Tenants.Queries.GetAllTenants;

public class GetAllTenantsHandler(
    ITenantRepository tenantRepository,
    ICacheService cacheService,
    ILogger<GetAllTenantsHandler> logger
) : IRequestHandler<GetAllTenantsQuery, PaginatedResult<TenantViewModel>>
{
    private const int MaxPageSize = 100;
    private readonly ITenantRepository _repo = tenantRepository;
    private readonly ICacheService _cache = cacheService;
    private readonly ILogger<GetAllTenantsHandler> _logger = logger;

    public async Task<PaginatedResult<TenantViewModel>> Handle(GetAllTenantsQuery query, CancellationToken ct)
    {
        ValidateQuery(query);

        int pageSize = Math.Min(query.PageSize, MaxPageSize);
        string cacheKey = BuildCacheKey(query, pageSize);

        var cached = await TryGetCachedResultAsync(cacheKey, ct);
        if (cached is not null) return cached;

        var result = await _repo.GetAllAsync(query.Search, query.By, query.Page, pageSize, ct);
        var mapped = result.Items.Select(MapToViewModel).ToList();

        var paginated = new PaginatedResult<TenantViewModel>
        {
            Items = mapped,
            PageNumber = query.Page,
            PageSize = pageSize,
            TotalCount = result.TotalCount
        };

        await _cache.SetAsync(cacheKey, paginated, TimeSpan.FromMinutes(5), ct);
        _logger?.LogInformation("Cached tenants for key: {CacheKey}", cacheKey);

        return paginated;
    }

    private static void ValidateQuery(GetAllTenantsQuery query)
    {
        if (query.Page < 1 || query.PageSize < 1)
            throw new BadRequestException("Page and pageSize must be greater than 0.");

        if (!string.IsNullOrWhiteSpace(query.Search) && string.IsNullOrWhiteSpace(query.By))
            throw new BadRequestException("Field-specific search requires a 'by' parameter.");
    }

    private static string BuildCacheKey(GetAllTenantsQuery query, int pageSize) =>
        $"tenants:{query.Search ?? "all"}:by:{query.By ?? "any"}:page:{query.Page}:size:{pageSize}";

    private async Task<PaginatedResult<TenantViewModel>?> TryGetCachedResultAsync(string key, CancellationToken ct)
    {
        _logger?.LogInformation("Checking cache for key: {CacheKey}", key);
        var cached = await _cache.GetAsync<PaginatedResult<TenantViewModel>>(key, ct);

        if (cached is not null)
            _logger?.LogInformation("Cache hit for key: {CacheKey}", key);

        return cached;
    }

    private static TenantViewModel MapToViewModel(Domain.Entities.Tenant t) => new()
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
    };
}
