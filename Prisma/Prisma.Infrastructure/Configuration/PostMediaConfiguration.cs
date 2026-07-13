using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class PostMediaConfiguration : IEntityTypeConfiguration<PostMedia>
{
    public void Configure(EntityTypeBuilder<PostMedia> builder)
    {
        builder.ToTable("post_medias");

        builder.HasKey(x => new { x.PostId, x.MediaId });

        builder.HasOne(x => x.Post)
             .WithMany(x => x.PostMedias)
             .HasForeignKey(x => x.PostId)
             .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Media)
             .WithMany(x => x.PostMedias)
             .HasForeignKey(x => x.MediaId)
             .OnDelete(DeleteBehavior.Cascade);
    }
}
