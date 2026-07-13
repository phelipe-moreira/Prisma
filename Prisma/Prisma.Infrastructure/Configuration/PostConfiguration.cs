using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder) 
    {
        builder.ToTable("posts");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Ngo)
             .WithMany(x => x.Posts)
             .HasForeignKey(x => x.NgoId)
             .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Content)
            .IsRequired(false)
            .HasColumnType("text");
    }
}
