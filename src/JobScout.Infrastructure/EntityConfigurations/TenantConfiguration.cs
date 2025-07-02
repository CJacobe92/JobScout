using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JobScout.Domain.Tenants;


namespace JobScout.Infrastructure.EntityConfigurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");
        builder.Property(t => t.Id)
        .HasConversion(
            id => id!.Value,
            value => new TenantId(value)
        );
        builder.Property("CompanyName").HasColumnName("CompanyName");
        builder.Property("ShardKey").HasColumnName("ShardKey");
        builder.Property("IsActivated").HasColumnName("IsActivated");
        builder.Property("WelcomeEmailSent").HasColumnName("WelcomeEmailSent");
        builder.Property("CreatedAt").HasColumnName("CreatedAt");
        builder.Property("UpdatedAt").HasColumnName("UpdatedAt");

    }
}

