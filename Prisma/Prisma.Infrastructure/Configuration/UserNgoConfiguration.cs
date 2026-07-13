using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class UserNgoConfiguration : IEntityTypeConfiguration<UserNgo>
{
    public void Configure(EntityTypeBuilder<UserNgo> builder)
    {
        builder.ToTable("user_ngos"); 

        builder.HasKey(x => new { x.UserId, x.NgoId }); 

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserNgos)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Ngo)
            .WithMany(x => x.UserNgos)
            .HasForeignKey(x => x.NgoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
