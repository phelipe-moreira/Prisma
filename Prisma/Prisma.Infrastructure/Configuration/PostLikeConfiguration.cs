using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
{
    public void Configure(EntityTypeBuilder<PostLike> builder)
    {
        builder.ToTable("post_likes");

        builder.HasKey(x => new { x.UserId, x.PostId });

        builder.HasOne(x => x.User)
             .WithMany(x => x.PostLikes)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Post)
             .WithMany(x => x.PostLikes)
             .HasForeignKey(x => x.PostId)
             .OnDelete(DeleteBehavior.Cascade);
    }
}
