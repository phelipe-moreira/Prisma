using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class MediaListConfiguration : IEntityTypeConfiguration<MediaList>
{
    public void Configure(EntityTypeBuilder<MediaList> builder)
    {
        builder.ToTable("media_lists");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany(x => x.MediaLists)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasColumnType("text");
    }
}
