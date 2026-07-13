using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255);         

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.ProfilePictureUrl)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(x => x.Bio)
            .IsRequired(false)
            .HasColumnType("text");
    }
}
