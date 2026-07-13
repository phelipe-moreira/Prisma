using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class NgoConfiguration : IEntityTypeConfiguration<Ngo>
{
    public void Configure(EntityTypeBuilder<Ngo> builder)
    {
        builder.ToTable("ngos");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Cnpj)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Cnpj)
            .IsRequired()
            .HasMaxLength(18);

        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasColumnType("text");

        builder.Property(x => x.ProfilePictureUrl)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(x => x.CoverPictureUrl)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(x => x.WebsiteUrl)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(x => x.InstagramUrl)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(x => x.ContactEmail)
            .IsRequired(false)
            .HasMaxLength(255);

        builder.Property(x => x.City)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(x => x.State)
            .IsRequired(false)
            .HasMaxLength(2)
            .IsFixedLength();
    }
}
