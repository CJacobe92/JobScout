using System;
using JobScout.Core.Commands.Tenant;
using JobScout.Core.Utilities;
using JobScout.Domain.Contracts;
using JobScout.Domain.Enumerations;
using JobScout.Domain.Models;
using MediatR;

namespace JobScout.Core.CommandHandlers.Tenant;

public class CreateTenantCommandHandler(
  ITenantRepository repo
) : IRequestHandler<CreateTenantCommand, Guid>
{

  public async Task<Guid> Handle(CreateTenantCommand command, CancellationToken ct)
  {

    var shardKey = Enum.Parse<AvailableShards>(ShardKeyGenerator.From(command.CompanyName));

    var domainTenant = new TenantModel(command.CompanyName, shardKey);

    var tenant = await repo.CreateTenant(
        command.FirstName,
        command.LastName,
        command.Email,
        command.Password,
        domainTenant,
        ct);

    return tenant.Id;
  }
}
