using System;
using TenantUserModel = JobScout.Domain.Models.TenantUser;
using TenantUserEntity = JobScout.Infrastructure.Database.Entities.TenantUser;

namespace JobScout.Infrastructure.Database.Mappings;

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
