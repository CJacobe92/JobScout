using System;
using JobScout.Core.Commands.Tenant;
using JobScout.Core.Exceptions;
using JobScout.Domain.Contracts;
using JobScout.Domain.Enumerations;
using JobScout.Domain.Models;
using MediatR;

namespace JobScout.Core.CommandHandlers.Tenant;

public class UpdateTenantCommandHandler(
  ITenantRepository repo
) : IRequestHandler<UpdateTenantCommand, TenantModel>
{
	public async Task<TenantModel> Handle(UpdateTenantCommand command, CancellationToken ct)
	{

		var tenant = await repo.GetOneById(command.Id, ct)
			?? throw new NotFoundException($"Tenant with ID: {command.Id}");

		var companyName = command.CompanyName ?? tenant.CompanyName;
		var shardKey = command.ShardKey ?? tenant.ShardKey;

		tenant.Update(companyName, shardKey);

		var updatedTenant = await repo.UpdateTenant(command.Id, tenant, ct)
			?? throw new BadRequestException($"Failed to update tenant with ID: {command.Id}");

		return updatedTenant;
	}
}
