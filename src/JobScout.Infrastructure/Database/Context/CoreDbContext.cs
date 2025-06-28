using TenantEntity = JobScout.Infrastructure.Database.Entities.Tenant;
using JobScout.Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobScout.Infrastructure.Database.Context
{
    public class CoreDbContext(DbContextOptions<CoreDbContext> options)
        : IdentityDbContext<CoreUser, IdentityRole<Guid>, Guid>(options)
    {
        public DbSet<TenantEntity> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>()
                .HasIndex(t => t.CompanyName)
                .IsUnique();

            modelBuilder.Entity<Tenant>()
                .HasIndex(t => t.ShardKey)
                .IsUnique();
        }
    };

}
