using System;
using JobScout.Domain.Contracts;
using JobScout.Domain.Enumerations;
using JobScout.Domain.Models;
using MediatR;

namespace JobScout.Core.Commands.Tenant;

public class UpdateTenantCommand(
  Guid Id,
  string?
  CompanyName,
  AvailableShards? ShardKey)
  : IRequest<TenantModel>
{
  public Guid Id { get; set; } = Id;
  public string? CompanyName { get; set; } = CompanyName;
  public AvailableShards? ShardKey { get; set; } = ShardKey;


}