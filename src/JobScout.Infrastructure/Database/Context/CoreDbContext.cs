using JobScout.Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobScout.Domain.Contracts;

namespace JobScout.Infrastructure.Database.Context
{
    public class CoreDbContext(DbContextOptions<CoreDbContext> options)
        : IdentityDbContext<CoreUser, IdentityRole<Guid>, Guid>(options)
    {
        public DbSet<TenantEntity> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TenantEntity>()
                .HasIndex(t => t.CompanyName)
                .IsUnique();

            modelBuilder.Entity<TenantEntity>()
                .HasIndex(t => t.ShardKey)
                .IsUnique();
        }

        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = now;

                if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt = now;
            }

            return base.SaveChangesAsync(ct);
        }

    };

}
