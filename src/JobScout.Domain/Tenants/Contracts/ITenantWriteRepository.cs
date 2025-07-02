using JobScout.Domain.SeedWork;

namespace JobScout.Domain.Tenants.Contracts
{
    public interface ITenantWriteRepository
    {
        Task<IResult<Tenant>> CreateTenantAsync(
            string CompanyName,
            string FirstName,
            string LastName,
            string Email,
            string Password,
            string ShardKey,
            CancellationToken ct
        );
    }
}