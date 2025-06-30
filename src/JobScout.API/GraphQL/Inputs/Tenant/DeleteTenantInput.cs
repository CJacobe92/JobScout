using System.ComponentModel.DataAnnotations;

namespace JobScout.API.GraphQL.Inputs.Tenant;

public record DeleteTenantInput(
    [Required] Guid Id
);
