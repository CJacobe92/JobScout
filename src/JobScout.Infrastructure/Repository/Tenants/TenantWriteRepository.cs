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
    UserManager<TenantUser> userManager,
    IDomainEventDispatcher dispatcher
) : ITenantWriteRepository
{
    private readonly IMediator _mediator = mediator;
    public async Task<IResult<Tenant>> CreateTenantAsync(
        string companyName,
        string firstName,
        string lastName,
        string email,
        string password,
        string shardKey,
        CancellationToken ct
    )
    {
        ct.ThrowIfCancellationRequested();

        using var trx = await context.Database.BeginTransactionAsync(ct);

        try
        {
            ct.ThrowIfCancellationRequested();

            var newTenant = Tenant.Create(companyName, shardKey);

            var newUser = TenantUser.Create(
                firstName,
                lastName,
                email,
                newTenant.Id
            );

            ct.ThrowIfCancellationRequested();

            var result = await userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                return Result<Tenant>.Failure($"Failed to create tenant. Errors: {errorMessages}");
            }


            ct.ThrowIfCancellationRequested();

            await context.Tenants.AddAsync(newTenant, ct);

            var tenantCreatedEvent = new TenantCreatedEvent(newTenant.Id, companyName);

            var domainEntities = context.ChangeTracker
                .Entries()
                .Where(e => e.Entity is Entity<object> entity && (entity.DomainEvents?.Count > 0))
                .Select(e => (Entity<object>)e.Entity)
                .ToList();

            await context.SaveChangesAsync(ct);
            await context.DispatchTrackedDomainEventsAsync(dispatcher, ct);
            await trx.CommitAsync(ct);

            return Result<Tenant>.Success(newTenant);
        }
        catch (System.Exception)
        {
            await trx.RollbackAsync(ct);
            throw;
        }
    }
}
