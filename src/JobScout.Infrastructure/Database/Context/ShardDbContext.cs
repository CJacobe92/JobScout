using System;
using JobScout.Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobScout.Infrastructure.Database.Context;

public class ShardDbContext(DbContextOptions<ShardDbContext> options)
  : IdentityDbContext<TenantUser, IdentityRole<Guid>, Guid>(options)
{

}
