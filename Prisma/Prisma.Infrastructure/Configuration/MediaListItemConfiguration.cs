using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class MediaListItemConfiguration : IEntityTypeConfiguration<MediaListItem>
{
    public void Configure(EntityTypeBuilder<MediaListItem> builder) 
    {
        builder.ToTable("media_list_items");

        builder.HasKey(x => new { x.MediaListId, x.MediaId });

        builder.HasOne(x => x.MediaList)
            .WithMany(x => x.MediaListItems)
            .HasForeignKey(x => x.MediaListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Media)
            .WithMany(x => x.MediaListItems)
            .HasForeignKey(x => x.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
