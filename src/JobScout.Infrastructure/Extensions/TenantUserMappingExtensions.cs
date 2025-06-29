using System;
using JobScout.Domain.Models;
using JobScout.Infrastructure.Database.Entities;


namespace JobScout.Infrastructure.Extensions;

public static class TenantUserMappingExtensions
{
  public static TenantUserEntity ToEntity(this TenantUserModel domain)
  {
    return new TenantUserEntity
    {
      FirstName = domain.FirstName,
      LastName = domain.LastName,
      Email = domain.Email,
      UserName = domain.Email,
      TenantId = domain.TenantId
    };
  }
}
