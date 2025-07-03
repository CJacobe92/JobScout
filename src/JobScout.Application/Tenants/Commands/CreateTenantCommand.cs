using System;
using JobScout.Domain.SeedWork;
using JobScout.Domain.Tenants;
using MediatR;

namespace JobScout.Application.Tenants.Commands;

public record CreateTenantCommand(
    string CompanyName,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    CancellationToken CancellationToken
) : IRequest<IResult<Tenant>>;
