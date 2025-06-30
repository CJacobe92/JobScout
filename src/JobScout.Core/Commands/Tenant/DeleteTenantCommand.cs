using System;
using MediatR;

namespace JobScout.Core.Commands.Tenant;

public class DeleteTenantCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
}