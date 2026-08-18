using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class UserNgoFollowConfiguration : IEntityTypeConfiguration<UserNgoFollow>
{
    public void Configure(EntityTypeBuilder<UserNgoFollow> builder)
    {
        builder.ToTable("user_ngo_follows");

        builder.HasKey(x => new { x.UserId, x.NgoId });

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserNgoFollows)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Ngo)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.NgoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
