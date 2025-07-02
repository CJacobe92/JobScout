using System;
using JobScout.Domain.Tenants;

namespace JobScout.API.GraphQL.Tenants.Output;

public record CreateTenantOutput(
    string Id,
    string CompanyName,
    string CreatedAt,
    string UpdatedAt
);