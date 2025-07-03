using System;
using JobScout.Domain.Tenants;

namespace JobScout.API.GraphQL.Tenants.Output;

public record TenantOutput(
    string Id,
    string CompanyName,
    string CreatedAt,
    string UpdatedAt
);