using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScout.Domain.Models;
using JobScout.Infrastructure.Database.Entities;

namespace JobScout.Infrastructure.Extensions
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
                CompanyName = domain.CompanyName,
                ShardKey = domain.ShardKey,
            };
        }
    }
}
