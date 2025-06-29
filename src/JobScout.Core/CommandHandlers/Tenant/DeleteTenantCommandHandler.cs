using System;
using JobScout.Core.Commands.Tenant;
using JobScout.Core.Exceptions;
using JobScout.Domain.Contracts;
using JobScout.Infrastructure.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobScout.Core.CommandHandlers.Tenant;

public class DeleteTenantCommandHandler(
    IUnitOfWork uow,
    UserManager<TenantUserEntity> userManager,
    ITenantRepository repo) : IRequestHandler<DeleteTenantCommand, Guid>
{
    public async Task<Guid> Handle(DeleteTenantCommand command, CancellationToken ct)
    {
        await uow.BeginTransactionAsync(ct);

        try
        {
            var existingTenant = await repo.GetOneById(command.Id, ct)
                ?? throw new NotFoundException($"Tenant with ID: {command.Id} not found");

            var users = await userManager.Users
                .Where(u => u.TenantId == existingTenant.Id)
                .ToListAsync(ct);

            foreach (var user in users)
            {
                var userResult = await userManager.DeleteAsync(user);
                if (!userResult.Succeeded)
                    throw new ConflictException("Failed to delete associated user(s): " +
                        string.Join(", ", userResult.Errors.Select(e => e.Description)));
            }

            var result = await repo.DeleteTenant(command.Id, ct)
                ?? throw new ConflictException("Failed to delete tenant");

            await uow.CommitAsync(ct);

            return result;
        }
        catch (Exception)
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}
