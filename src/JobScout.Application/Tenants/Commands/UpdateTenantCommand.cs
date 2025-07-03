using System;
using System.ComponentModel.DataAnnotations;
using JobScout.Domain.SeedWork;
using JobScout.Domain.Tenants;
using MediatR;

namespace JobScout.Application.Tenants.Commands;

public record UpdateTenantCommand(
    string? CompanyName,
    CancellationToken CancellationToken
) : IRequest<IResult<Tenant>>;
