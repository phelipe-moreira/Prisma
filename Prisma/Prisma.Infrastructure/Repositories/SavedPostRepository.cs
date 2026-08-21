using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories
{
    public class SavedPostRepository(AppDbContext context) : ISavedPostRepository
    {
        public async Task AddAsync(SavedPost savedPost, CancellationToken cancellationToken = default)
        {
            await context.SavedPosts.AddAsync(savedPost, cancellationToken);
        }

        public Task DeleteAsync(SavedPost savedPost, CancellationToken cancellationToken = default)
        {
            context.SavedPosts.Remove(savedPost);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
        {
            return await context.SavedPosts.AnyAsync(x => x.UserId == userId && x.PostId == postId, cancellationToken);
        }

        public async Task<SavedPost?> GetAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
        {
            return await context.SavedPosts.FirstOrDefaultAsync(x => x.UserId == userId && x.PostId == postId, cancellationToken);
        }
    }
}
