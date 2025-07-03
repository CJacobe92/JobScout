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
    AppDbContext context,
    IMediator mediator,
    IDomainEventDispatcher dispatcher,
    ISlugResolver slugResolver,
    TenantProvisioner provisioner
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

        using var trx = await context.Database.BeginTransactionAsync(ct);

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
            await context.Tenants.AddAsync(newTenant, ct);

            // Step 7: Fire domain events & commit
            await context.SaveChangesAsync(ct);
            await context.DispatchTrackedDomainEventsAsync(dispatcher, ct);
            await trx.CommitAsync(ct);

            return Result<Tenant>.Success(newTenant);
        }
        catch (Exception)
        {
            await trx.RollbackAsync(ct);
            throw;
        }
    }
}
