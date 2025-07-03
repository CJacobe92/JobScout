using JobScout.Domain.Outbox;
using JobScout.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobScout.Infrastructure.EntityConfigurations;

public class OutboxConfiguration : IEntityTypeConfiguration<Outbox>
{
    public void Configure(EntityTypeBuilder<Outbox> builder)
    {
        builder.ToTable("Outbox");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.EventType)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(m => m.ContentJson)
            .IsRequired();

        builder.Property(m => m.CreatedOn)
            .IsRequired();

        builder.Property(m => m.ProcessedOn);
    }
}