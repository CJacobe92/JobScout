using System;
using JobScout.Core.Commands.Tenant;
using JobScout.Core.Exceptions;
using JobScout.Domain.Contracts;
using JobScout.Domain.Models;
using MediatR;

namespace JobScout.Core.CommandHandlers.Tenant;

public class UpdateTenantCommandHandler(
  IUnitOfWork unitOfWork,
  ITenantRepository tenantRepository
) : IRequestHandler<UpdateTenantCommand, TenantModel>
{
	private readonly IUnitOfWork _uow = unitOfWork;
	private readonly ITenantRepository _repo = tenantRepository;

	public async Task<TenantModel> Handle(UpdateTenantCommand command, CancellationToken ct)
	{
		await _uow.BeginTransactionAsync(ct);

		try
		{
			var existingTenant = await _repo.GetOneById(command.Id)
				?? throw new NotFoundException($"Tenant with ID: {command.Id} not found");

			existingTenant.Update(command.CompanyName, command.ShardKey);

			var result = await _repo.UpdateTenant(command.Id, existingTenant)
				?? throw new ConflictException("Failed to update tenant");

			await _uow.CommitAsync(ct);

			return result;
		}
		catch (Exception)
		{
			await _uow.RollbackAsync(ct);
			throw;
		}
	}
}
