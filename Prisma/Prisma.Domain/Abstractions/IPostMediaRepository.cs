using Prisma.Domain.Entities;

namespace Prisma.Domain.Abstractions;

public interface IPostMediaRepository
{
    Task AddAsync(PostMedia postMedia, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Guid>> GetPostIdsByMediaIdAsync(
        Guid mediaId,
        CancellationToken cancellationToken = default);
}
