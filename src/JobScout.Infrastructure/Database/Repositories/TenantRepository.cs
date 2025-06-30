using JobScout.Domain.Contracts;
using JobScout.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using JobScout.Infrastructure.Extensions;
using JobScout.Domain.Models;
using Microsoft.AspNetCore.Identity;
using JobScout.Infrastructure.Database.Entities;

namespace JobScout.Infrastructure.Database.Repositories
{
    public class TenantRepository(
        CoreDbContext context,
        UserManager<TenantUserEntity> userManager
        ) : ITenantRepository
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
            var entity = await context.Tenants.FindAsync([id], ct);
            return entity?.ToDomain();
        }

        public async Task<TenantModel> CreateTenant(
            string firstName,
            string lastName,
            string email,
            string password,
            TenantModel tenantModel,
            CancellationToken ct)
        {
            using var trx = await context.Database.BeginTransactionAsync(ct);

            try
            {
                var entity = tenantModel.ToEntity();

                await context.AddAsync(entity, ct);
                await context.SaveChangesAsync(ct);

                var newUser = new TenantUserEntity
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    TenantId = entity.Id
                };

                var result = await userManager.CreateAsync(newUser, password);

                if (!result.Succeeded)
                    throw new Exception("User creation failed: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));

                await trx.CommitAsync(ct);
                return entity.ToDomain();
            }
            catch (Exception)
            {
                await trx.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<TenantModel?> UpdateTenant(Guid id, TenantModel tenantModel, CancellationToken ct)
        {
            var entity = await FindTenantById(id, ct)
                ?? throw new InvalidOperationException($"Failed to update tenant with ID: {id}");

            entity.CompanyName = tenantModel.CompanyName;
            entity.ShardKey = tenantModel.ShardKey.ToString();
            await context.SaveChangesAsync(ct);
            return entity.ToDomain();
        }

        public async Task<Guid?> DeleteTenant(Guid id, CancellationToken ct)
        {
            var entity = await FindTenantById(id, ct);
            if (entity is null) return null;

            var users = await userManager
                .Users
                .Where(u => u.TenantId == id)
                .ToListAsync(ct);

            foreach (var user in users)
            {
                var result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    throw new ArgumentException("Failed to delete associated user(s): " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            context.Tenants.Remove(entity);
            await context.SaveChangesAsync(ct);

            return entity.ToDomain().Id;
        }

        private async Task<TenantEntity?> FindTenantById(Guid id, CancellationToken ct)
        {
            return await context.Tenants.FindAsync([id], ct);
        }
    }
}
