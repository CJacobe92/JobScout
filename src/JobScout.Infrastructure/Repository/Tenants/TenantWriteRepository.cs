using System;
using JobScout.Infrastructure.Extensions;
using JobScout.Domain.SeedWork;
using JobScout.Domain.Tenants;
using JobScout.Domain.Tenants.Contracts;
using JobScout.Infrastructure.Database;
using JobScout.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using MediatR;

namespace JobScout.Infrastructure.Repository.Tenants;

public class TenantWriteRepository(
    AppDbContext appDbContext,
    TenantDbContext tenantDbContext,
    IMediator mediator,
    IDomainEventDispatcher dispatcher,
    ISlugResolver slugResolver,
    TenantProvisioner provisioner,
    ITenantContext tenantContext
    ) : ITenantWriteRepository
{
    private readonly IMediator _mediator = mediator;

    public async Task<IResult<Tenant>> CreateTenantAsync(
        string companyName,
        string firstName,
        string lastName,
        string email,
        string password,
        CancellationToken ct
    )
    {
        ct.ThrowIfCancellationRequested();

        using var trx = await appDbContext.Database.BeginTransactionAsync(ct);

        try
        {
            // Step 1: Generate slug from company name
            var configuredSlug = slugResolver.ResolveFor(companyName);

            // Step 4: Create domain tenant and user
            var newTenant = Tenant.Create(companyName, configuredSlug);
            var newUser = TenantUser.Create(firstName, lastName, email, newTenant.Id);

            var tenantId = newTenant.Id.ToString();

            await provisioner.EnsureSchemaCreatedAsync(newUser, password, tenantId, configuredSlug, ct);

            // Step 6: Persist tenant globally
            await appDbContext.Tenants.AddAsync(newTenant, ct);

            // Step 7: Fire domain events & commit
            await appDbContext.SaveChangesAsync(ct);
            await appDbContext.DispatchTrackedDomainEventsAsync(dispatcher, ct);
            await trx.CommitAsync(ct);

            return Result<Tenant>.Success(newTenant);
        }
        catch (Exception)
        {
            await trx.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<IResult<Tenant>> UpdateTenantAsync(
        string? companyName,
        CancellationToken ct
    )
    {
        ct.ThrowIfCancellationRequested();
        using var trx = await tenantDbContext.Database.BeginTransactionAsync(ct);
        var tenantId = tenantContext.TenantId;

        try
        {
            var foundTenant = await FindTenantAsync(tenantId, ct);

            if (foundTenant is null)
            {
                throw new ArgumentException("Tenant not found");
            }

            foundTenant.ChangeCompanyName(companyName!);
            return Result<Tenant>.Success(foundTenant);
        }
        catch (System.Exception)
        {

            return Result<Tenant>.Failure("Failed to update tenant");
        }
    }

    private async Task<Tenant?> FindTenantAsync(TenantId id, CancellationToken ct)
    {
        return await appDbContext.Tenants.FindAsync(id, ct);
    }


}
