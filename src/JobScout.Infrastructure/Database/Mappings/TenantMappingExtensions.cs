using TenantModel = JobScout.Domain.Models.Tenant;
using TenantEntity = JobScout.Infrastructure.Database.Entities.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Infrastructure.Database.Mappings
{
    public static class TenantMappingExtensions
    {
        public static TenantModel ToDomain(this TenantEntity entity)
        {
            return new TenantModel(
                entity.Id,
                entity.CompanyName,
                entity.ShardKey
            );
        }

        public static TenantEntity ToEntity(this TenantModel domain)
        {
            return new TenantEntity
            {
                Id = domain.Id,
                CompanyName = domain.CompanyName?.Value ?? string.Empty,
                ShardKey = domain.ShardKey.Value,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt
            };
        }
    }
}
