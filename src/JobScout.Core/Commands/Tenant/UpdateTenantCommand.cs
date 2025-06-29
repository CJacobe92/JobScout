using System;
using JobScout.Domain.Models;
using MediatR;

namespace JobScout.Core.Commands.Tenant;

public record UpdateTenantCommand : IRequest<TenantModel>
{
  public Guid Id { get; set; }
  public string? CompanyName { get; set; }
  public string? ShardKey { get; set; }
}