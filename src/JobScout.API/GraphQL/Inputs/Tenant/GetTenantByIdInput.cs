using System.ComponentModel.DataAnnotations;

namespace JobScout.API.GraphQL.Inputs.Tenant;

public record GetTenantByIdInput(
    [Required] Guid Id
);
