using JobScout.Domain.Models;
using JobScout.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Contracts
{
    public interface ITenantRepository
    {
        Task<IEnumerable<Tenant>> GetAll();
        Task<IEnumerable<Tenant>> GetAllByShardKey(string shard);
        Task<Tenant?> GetOneById(Guid id);
        Task CreateTenant(Tenant tenant);
        Task UpdateTenant(Tenant tenant);
        Task DeleteTenant(Guid id);
    }
}
