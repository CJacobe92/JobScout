using JobScout.Domain.Contracts;
using JobScout.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using JobScout.Infrastructure.Extensions;
using JobScout.Domain.Models;

namespace JobScout.Infrastructure.Database.Repositories
{
    public class TenantRepository(CoreDbContext context) : ITenantRepository
    {
        public async Task<IEnumerable<TenantModel>> GetAll(CancellationToken ct)
        {
            return (
                await context.Tenants
                    .AsNoTracking()
                    .ToListAsync(ct))
                    .Select(e => e.ToDomain());
        }

        public async Task<IEnumerable<TenantModel>> GetAllByShardKey(string shard, CancellationToken ct)
        {
            return (
                await context.Tenants
                    .AsNoTracking()
                    .Where(t => t.ShardKey == shard)
                    .ToListAsync(ct))
                    .Select(e => e.ToDomain());
        }

        public async Task<TenantModel?> GetOneById(Guid id, CancellationToken ct)
        {
            var entity = await context.Tenants.FindAsync(id, ct);
            return entity?.ToDomain();
        }

        public async Task<TenantModel> CreateTenant(TenantModel tenantModel, CancellationToken ct)
        {
            var entity = tenantModel.ToEntity();
            await context.AddAsync(entity, ct);
            await context.SaveChangesAsync(ct);

            return entity.ToDomain();
        }

        public async Task<TenantModel?> UpdateTenant(Guid id, TenantModel tenantModel, CancellationToken ct)
        {
            var entity = await FindTenantById(id, ct);
            if (entity is null) return null;

            entity.CompanyName = tenantModel.CompanyName;
            entity.ShardKey = tenantModel.ShardKey;
            await context.SaveChangesAsync(ct);
            return entity.ToDomain();
        }

        public async Task<Guid?> DeleteTenant(Guid id, CancellationToken ct)
        {
            var entity = await FindTenantById(id, ct);
            if (entity is null) return null;

            context.Tenants.Remove(entity);
            await context.SaveChangesAsync(ct);

            return entity.ToDomain().Id;
        }

        private async Task<Entities.TenantEntity?> FindTenantById(Guid id, CancellationToken ct)
        {
            return await context.Tenants.FindAsync([id], cancellationToken: ct);
        }
    }
}
