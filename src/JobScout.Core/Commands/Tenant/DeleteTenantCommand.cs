using System;
using MediatR;

namespace JobScout.Core.Commands.Tenant;

public record DeleteTenantCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
}