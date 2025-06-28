using TenantModel = JobScout.Domain.Models.Tenant;
using JobScout.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScout.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using JobScout.Infrastructure.Database.Mappings;

namespace JobScout.Infrastructure.Database.Repositories
{
    public class TenantRepository(CoreDbContext _context): ITenantRepository
    {
        private readonly CoreDbContext context = _context;

        public async Task<IEnumerable<TenantModel>> GetAll()
        {
            return (
                await context.Tenants
                    .AsNoTracking()
                    .ToListAsync())
                    .Select(e => e.ToDomain());
        }

        public async Task<IEnumerable<TenantModel>> GetAllByShardKey(string shard)
        {
            return (
                await context.Tenants
                    .AsNoTracking()
                    .Where(t => t.ShardKey == shard)
                    .ToListAsync())
                    .Select(e => e.ToDomain());
        }

        public async Task<TenantModel?> GetOneById(Guid id)
        {
            var entity = await context.Tenants.FindAsync(id);
            return entity?.ToDomain();
        }

        public async Task<Guid> CreateTenant(TenantModel tenantModel)
        {
            var entity = tenantModel.ToEntity();
            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateTenant(TenantModel tenantModel)
        {
            var entity = tenantModel.ToEntity();
            context.Tenants.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteTenant(Guid id)
        {
            var tenant = await context.Tenants.FindAsync(id);
            if (tenant != null)
            {
                context.Tenants.Remove(tenant);
                await context.SaveChangesAsync();
            }
        }
    }
}
