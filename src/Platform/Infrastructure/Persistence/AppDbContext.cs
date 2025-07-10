using System;
using Domain.Entities;
using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Shared.SeedWork;


namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Automatically applies all IEntityTypeConfiguration<T> in the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents != null && x.DomainEvents.Count != 0)
            .ToList();

        foreach (var entity in domainEntities)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                this.AddOutboxEvent(domainEvent);
            }

            entity.ClearDomainEvents();
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
