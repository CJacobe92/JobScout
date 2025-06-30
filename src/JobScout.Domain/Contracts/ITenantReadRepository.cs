using JobScout.Domain.Models;

namespace JobScout.Domain.Contracts;

public interface ITenantReadRepository
{
    Task<IEnumerable<TenantModel>> GetAllAsync(CancellationToken ct);
}
