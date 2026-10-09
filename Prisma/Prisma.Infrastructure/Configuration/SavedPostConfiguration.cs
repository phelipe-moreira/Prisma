using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class SavedPostConfiguration : IEntityTypeConfiguration<SavedPost>
{
    public void Configure(EntityTypeBuilder<SavedPost> builder)
    {
        builder.ToTable("saved_posts");

        builder.HasKey(x => new { x.UserId, x.PostId });

        builder.HasOne(x => x.User)
             .WithMany(x => x.SavedPosts)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Post)
             .WithMany(x => x.SavedPosts)
             .HasForeignKey(x => x.PostId)
             .OnDelete(DeleteBehavior.Cascade);
    }
}
