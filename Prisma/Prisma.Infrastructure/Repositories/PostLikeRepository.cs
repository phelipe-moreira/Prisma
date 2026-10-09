using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories
{
    public class PostLikeRepository(AppDbContext context) : IPostLikeRepository
    {
        public async Task AddAsync(PostLike postLike, CancellationToken cancellationToken = default)
        {
            await context.PostLikes.AddAsync(postLike, cancellationToken);
        }

        public Task DeleteAsync(PostLike postLike, CancellationToken cancellationToken = default)
        {
            context.PostLikes.Remove(postLike);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
        {
            return await context.PostLikes.AnyAsync(x => x.UserId == userId && x.PostId == postId, cancellationToken);
        }

        public async Task<PostLike?> GetAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
        {
            return await context.PostLikes.FirstOrDefaultAsync(x => x.UserId == userId && x.PostId == postId, cancellationToken);
        }
    }
}
