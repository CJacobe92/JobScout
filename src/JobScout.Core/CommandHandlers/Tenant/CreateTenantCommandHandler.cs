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
  IUnitOfWork unitOfWork,
  ITenantRepository tenantRepository,
  UserManager<TenantUserEntity> userManager
) : IRequestHandler<CreateTenantCommand, Guid>
{
    private readonly IUnitOfWork _uow = unitOfWork;
    private readonly ITenantRepository _repo = tenantRepository;

    public async Task<Guid> Handle(CreateTenantCommand command, CancellationToken ct)
    {
        await _uow.BeginTransactionAsync(ct);

        try
        {
            var domainTenant = new TenantModel(
              command.CompanyName,
              ShardKeyGenerator.From(command.CompanyName)
            );

            var tenant = await _repo.CreateTenant(domainTenant);

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

            await _uow.CommitAsync(ct);
            return tenant.Id;
        }
        catch (System.Exception)
        {
            await _uow.RollbackAsync(ct);
            throw;
        }
    }
}
