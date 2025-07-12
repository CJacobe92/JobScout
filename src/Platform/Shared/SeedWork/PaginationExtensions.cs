using System;
using Microsoft.EntityFrameworkCore;
using Shared.SeedWork;

namespace Shared.SeedWork;

public static class PaginationExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PaginatedResult<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    // Uncomment when you're ready for projection support 👇
    /*
    public static async Task<PaginatedResult<TOut>> ToPaginatedResultAsync<TIn, TOut>(
        this IQueryable<TIn> query,
        int pageNumber,
        int pageSize,
        Func<TIn, TOut> selector,
        CancellationToken ct = default)
    {
        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(selector) // projection layer
            .ToListAsync(ct);

        return new PaginatedResult<TOut>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
    */
}
