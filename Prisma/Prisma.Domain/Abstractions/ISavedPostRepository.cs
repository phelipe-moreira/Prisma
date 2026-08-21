using Prisma.Domain.Entities;

namespace Prisma.Domain.Abstractions
{
    public interface ISavedPostRepository
    {
        Task AddAsync(SavedPost savedPost, CancellationToken cancellationToken = default);
        Task DeleteAsync(SavedPost savedPost, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default);
        Task<SavedPost?> GetAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default);
    }
}
