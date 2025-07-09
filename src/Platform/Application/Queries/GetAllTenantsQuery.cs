using System;

namespace Application.Queries;

public record GetAllTenantsQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null);

