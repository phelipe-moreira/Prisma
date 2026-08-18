using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Entities;

namespace Prisma.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cause> Causes { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserNgo> UserNgos { get; set; }

    public DbSet<UserNgoFollow> UserNgoFollows { get; set; }

    public DbSet<Ngo> Ngo { get; set; }

    public DbSet<NgoCause> NgoCauses { get; set; }

    public DbSet<Media> Medias { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<PostLike> PostLikes { get; set; }

    public DbSet<PostLink> PostLinks { get; set; }

    public DbSet<PostMedia> PostMedias { get; set; }

    public DbSet<SavedPost> SavedPosts { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
