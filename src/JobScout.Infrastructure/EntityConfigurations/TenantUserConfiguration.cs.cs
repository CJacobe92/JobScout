using JobScout.Domain.Tenants;
using JobScout.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Infrastructure.EntityConfigurations;


public class TenantUserConfiguration : IEntityTypeConfiguration<TenantUser>
{
    public void Configure(EntityTypeBuilder<TenantUser> builder)
    {
        builder.ToTable("AspNetUsers");
        builder.Property("FirstName").HasColumnName("FirstName");
        builder.Property("LastName").HasColumnName("LastName");
        builder.Property("RefreshToken").HasColumnName("RefreshToken");
        builder.Property("RefreshTokenExpiry").HasColumnName("RefreshTokenExpiry");
        builder.Property("CreatedAt").HasColumnName("CreatedAt");
        builder.Property("UpdatedAt").HasColumnName("UpdatedAt");
        builder.Property(u => u.TenantId)
        .HasConversion(
            id => id.Value,
            value => new TenantId(value)
        );
    }
}

