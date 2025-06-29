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
        Task<IEnumerable<TenantModel>> GetAll();
        Task<IEnumerable<TenantModel>> GetAllByShardKey(string shard);
        Task<TenantModel?> GetOneById(Guid id);
        Task<TenantModel> CreateTenant(TenantModel tenant);
        Task<TenantModel?> UpdateTenant(Guid id, TenantModel tenant);
        Task<Guid?> DeleteTenant(Guid id);
    }
}
