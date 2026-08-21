using Prisma.Domain.Entities;

namespace Prisma.Domain.Abstractions;

public interface IMediaRepository
{
    Task<Media?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task AddAsync(Media media, CancellationToken cancellationToken);

    Task Remove(Media media);
}
