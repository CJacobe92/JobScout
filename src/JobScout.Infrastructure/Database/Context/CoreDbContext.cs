using TenantEntity =  JobScout.Infrastructure.Database.Entities.Tenant;
using JobScout.Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobScout.Infrastructure.Database.Context
{
    public class CoreDbContext(DbContextOptions<CoreDbContext> options) 
        : IdentityDbContext<CoreUser, IdentityRole<Guid>, Guid>(options)
    {
        public DbSet<TenantEntity> Tenants { get; set; }
    };

}
