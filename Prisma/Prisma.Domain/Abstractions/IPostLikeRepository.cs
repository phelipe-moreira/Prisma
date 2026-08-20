using Prisma.Domain.Entities;

namespace Prisma.Domain.Abstractions
{
    public interface IPostLikeRepository
    {
        Task<bool> ExistsAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default);
        Task<PostLike?> GetAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default);
        Task AddAsync(PostLike postLike, CancellationToken cancellationToken = default);
        Task DeleteAsync(PostLike postLike, CancellationToken cancellationToken = default);
    }
}
