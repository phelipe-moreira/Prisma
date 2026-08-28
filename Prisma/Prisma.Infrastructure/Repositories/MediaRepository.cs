using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories;

public class MediaRepository(AppDbContext context) : IMediaRepository
{
    public async Task<Media?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Medias
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(Media media, CancellationToken cancellationToken)
    {
        await context.Medias.AddAsync(
            media,
            cancellationToken);
    }

    public Task Delete(Media media)
    {
        context.Medias.Remove(media);

        return Task.CompletedTask;
    }
}
