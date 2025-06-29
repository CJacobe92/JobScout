using System;
using JobScout.Core.Commands.Tenant;
using JobScout.Core.Exceptions;
using JobScout.Core.Utilities;
using JobScout.Domain.Contracts;
using JobScout.Domain.Models;
using JobScout.Infrastructure.Database.Entities;
using JobScout.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace JobScout.Core.CommandHandlers.Tenant;

public class CreateTenantCommandHandler(
  IUnitOfWork uow,
  ITenantRepository repo,
  UserManager<TenantUserEntity> userManager
) : IRequestHandler<CreateTenantCommand, Guid>
{

    public async Task<Guid> Handle(CreateTenantCommand command, CancellationToken ct)
    {
        await uow.BeginTransactionAsync(ct);

        try
        {
            var domainTenant = new TenantModel(
              command.CompanyName,
              ShardKeyGenerator.From(command.CompanyName)
            );

            var tenant = await repo.CreateTenant(domainTenant, ct);

            var domainUser = new TenantUserModel(
                command.FirstName,
                command.LastName,
                command.Email,
                command.Email,
                tenant.Id
            );

            var entityUser = domainUser.ToEntity();

            var result = await userManager.CreateAsync(entityUser);

            if (!result.Succeeded)
                throw new ConflictException("User creation failed: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            await uow.CommitAsync(ct);
            return tenant.Id;
        }
        catch (Exception)
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}
