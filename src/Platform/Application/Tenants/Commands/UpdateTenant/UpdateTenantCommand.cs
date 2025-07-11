using Domain.Entities;
using MediatR;

namespace Application.Tenants.Commands.UpdateTenant;

public record UpdateTenantCommand(
    Guid Id,
    string? Name,
    string? License,
    string? Phone,
    string? RegisteredTo,
    string? TIN,
    string? Address
) : IRequest<Tenant>;
