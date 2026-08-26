using Prisma.Domain.Entities;

namespace Prisma.Domain.Abstractions;

public interface IPostMediaRepository
{
    Task<PostMedia?> GetAsync(Guid postId, Guid mediaId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid postId, Guid mediaId, CancellationToken cancellationToken);
    Task AddAsync(PostMedia postMedia, CancellationToken cancellationToken = default);
    Task DeleteAsync(PostMedia postMedia, CancellationToken cancellationToken = default);
}
