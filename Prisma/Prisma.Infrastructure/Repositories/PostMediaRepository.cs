using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Prisma.Infrastructure.Repositories;

public class PostMediaRepository(AppDbContext context) : IPostMediaRepository
{
    public async Task AddAsync(PostMedia postMedia, CancellationToken cancellationToken = default)
        => await context.PostMedias.AddAsync(postMedia, cancellationToken);

    public async Task<IReadOnlyCollection<Guid>> GetPostIdsByMediaIdAsync(
        Guid mediaId,
        CancellationToken cancellationToken = default)
        => await context.PostMedias
            .Where(x => x.MediaId == mediaId)
            .Select(x => x.PostId)
            .ToListAsync(cancellationToken);
}
