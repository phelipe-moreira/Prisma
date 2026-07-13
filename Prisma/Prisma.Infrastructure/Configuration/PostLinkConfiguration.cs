using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class PostLinkConfiguration : IEntityTypeConfiguration<PostLink>
{
    public void Configure(EntityTypeBuilder<PostLink> builder)
    {
        builder.ToTable("post_links");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Post)
             .WithMany(x => x.PostLinks)
             .HasForeignKey(x => x.PostId)
             .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.LinkTitle)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(500);
    }
}
