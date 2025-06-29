using System;
using JobScout.Core.Commands.Tenant;
using JobScout.Core.Exceptions;
using JobScout.Domain.Contracts;
using MediatR;

namespace JobScout.Core.CommandHandlers.Tenant;

public class DeleteTenantCommandHandler(
    IUnitOfWork uow,
    ITenantRepository repo) : IRequestHandler<DeleteTenantCommand, Guid>
{
    public async Task<Guid> Handle(DeleteTenantCommand command, CancellationToken ct)
    {
        await uow.BeginTransactionAsync(ct);

        try
        {
            var existingTenant = await repo.GetOneById(command.Id)
                ?? throw new NotFoundException($"Tenant with ID: {command.Id} not found");

            var result = await repo.DeleteTenant(command.Id)
                ?? throw new ConflictException("Failed to delete tenant");

            await uow.CommitAsync(ct);

            return result;
        }
        catch (System.Exception)
        {

            throw;
        }
    }
}
