using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Configuration;

public class NgoCauseConfiguration : IEntityTypeConfiguration<NgoCause>
{
    public void Configure(EntityTypeBuilder<NgoCause> builder)
    {
        builder.ToTable("ngo_causes");

        builder.HasKey(x => new { x.NgoId, x.CauseId });

        builder.HasOne(x => x.Ngo)
            .WithMany(x => x.NgoCauses)
            .HasForeignKey(x => x.NgoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Cause)
            .WithMany(x => x.NgoCauses)
            .HasForeignKey(x => x.CauseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
