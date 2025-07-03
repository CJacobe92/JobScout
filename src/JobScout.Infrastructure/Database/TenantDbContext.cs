using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScout.Domain.Tenants;
using JobScout.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using JobScout.Infrastructure.EntityConfigurations;

namespace JobScout.Infrastructure.Database
{
    public class TenantDbContext : IdentityDbContext<TenantUser, IdentityRole<Guid>, Guid>
    {
        public readonly string _schema;
        public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TenantUserConfiguration());

        }
    }
}
