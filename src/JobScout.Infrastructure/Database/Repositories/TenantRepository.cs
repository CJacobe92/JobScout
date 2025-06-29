using JobScout.Domain.Contracts;
using JobScout.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using JobScout.Infrastructure.Extensions;
using JobScout.Domain.Models;

namespace JobScout.Infrastructure.Database.Repositories
{
    public class TenantRepository(CoreDbContext context) : ITenantRepository
    {
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

        public async Task<TenantModel> CreateTenant(TenantModel tenantModel)
        {
            var entity = tenantModel.ToEntity();
            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.ToDomain();
        }

        public async Task<TenantModel?> UpdateTenant(Guid id, TenantModel tenantModel)
        {
            var entity = await FindTenantById(id);
            if (entity is null) return null;

            entity.CompanyName = tenantModel.CompanyName;
            entity.ShardKey = tenantModel.ShardKey;
            await context.SaveChangesAsync();
            return entity.ToDomain();
        }

        public async Task<Guid?> DeleteTenant(Guid id)
        {
            var entity = await FindTenantById(id);
            if (entity is null) return null;

            context.Tenants.Remove(entity);
            await context.SaveChangesAsync();

            return entity.Id;
        }

        private async Task<Entities.TenantEntity?> FindTenantById(Guid id)
        {
            return await context.Tenants.FindAsync(id);
        }
    }
}
