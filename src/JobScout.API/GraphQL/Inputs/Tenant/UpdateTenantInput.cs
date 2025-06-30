using System.ComponentModel.DataAnnotations;

namespace JobScout.API.GraphQL.Inputs.Tenant;

public record UpdateTenantInput(
    [Required] Guid Id,
    string? CompanyName,
    string? Shard
);
