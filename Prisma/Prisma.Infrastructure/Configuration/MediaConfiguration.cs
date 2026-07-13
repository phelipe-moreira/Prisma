using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.ToTable("media");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ExternalId)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasColumnType("text");

        builder.Property(x => x.ImageUrl)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(x => x.LinkUrl)
            .IsRequired(false)
            .HasMaxLength(500);
    }
}
