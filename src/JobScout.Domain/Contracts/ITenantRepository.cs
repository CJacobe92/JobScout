using JobScout.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Contracts
{
    public interface ITenantRepository
    {
        Task<IEnumerable<TenantModel>> GetAll(CancellationToken ct = default);
        Task<IEnumerable<TenantModel>> GetAllByShardKey(string shard, CancellationToken ct = default);
        Task<TenantModel?> GetOneById(Guid id, CancellationToken ct = default);
        Task<TenantModel> CreateTenant(
            string FirstName,
            string LastName,
            string Email,
            string Password,
            TenantModel tenant,
            CancellationToken ct = default);
        Task<TenantModel?> UpdateTenant(Guid id, TenantModel tenant, CancellationToken ct = default);
        Task<Guid?> DeleteTenant(Guid id, CancellationToken ct = default);
    }
}
