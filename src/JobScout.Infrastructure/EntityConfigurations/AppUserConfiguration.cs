using JobScout.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Infrastructure.EntityConfigurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AspNetUsers");
        builder.Property("FirstName").HasColumnName("FirstName");
        builder.Property("LastName").HasColumnName("LastName");
        builder.Property("RefreshToken").HasColumnName("RefreshToken");
        builder.Property("RefreshTokenExpiry").HasColumnName("RefreshTokenExpiry");
        builder.Property("CreatedAt").HasColumnName("CreatedAt");
        builder.Property("UpdatedAt").HasColumnName("UpdatedAt");
    }
}

