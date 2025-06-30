using System;
using JobScout.Core.Commands.Tenant;
using JobScout.Domain.Contracts;
using MediatR;

namespace JobScout.Core.CommandHandlers.Tenant;

public class DeleteTenantCommandHandler(
    ITenantRepository repo) : IRequestHandler<DeleteTenantCommand, Guid>
{
    public async Task<Guid> Handle(DeleteTenantCommand command, CancellationToken ct)
    {
        await repo.GetOneById(command.Id, ct);

        var deletedId = await repo.DeleteTenant(command.Id, ct);

        return (Guid)deletedId;

    }
}
