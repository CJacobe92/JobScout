using JobScout.Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobScout.Domain.Contracts;

namespace JobScout.Infrastructure.Database.Context;

public class CoreDbContext : IdentityDbContext<CoreUser, IdentityRole<Guid>, Guid>
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options)
        : base(options) { }

    public DbSet<TenantEntity> Tenants => Set<TenantEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TenantEntity>()
            .HasIndex(x => x.CompanyName)
            .IsUnique();

        builder.Entity<TenantEntity>()
            .HasIndex(x => x.ShardKey)
            .IsUnique();
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }

        }

        return base.SaveChangesAsync(ct);
    }
}

