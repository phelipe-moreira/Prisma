using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories;

public class PostMediaRepository(AppDbContext context) : IPostMediaRepository
{
    public async Task AddAsync(PostMedia postMedia, CancellationToken cancellationToken = default)
    {
        await context.PostMedias.AddAsync(postMedia, cancellationToken);
    }

    public Task DeleteAsync(PostMedia postMedia, CancellationToken cancellationToken = default)
    {
        context.PostMedias.Remove(postMedia);

        return Task.CompletedTask;
    }

    public async Task<PostMedia?> GetAsync(Guid postId, Guid mediaId, CancellationToken cancellationToken)
    {
        return await context.PostMedias
            .FirstOrDefaultAsync(
            x => x.PostId == postId &&
                     x.MediaId == mediaId,
                cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid postId, Guid mediaId, CancellationToken cancellationToken)
    {
        return await context.PostMedias
            .AnyAsync(
            x => x.PostId == postId &&
                     x.MediaId == mediaId,
                cancellationToken);
    }
}
