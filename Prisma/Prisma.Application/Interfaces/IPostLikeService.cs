using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces
{
    public interface IPostLikeService
    {
        Task<Result> LikeAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default);
        Task<Result> UnlikeAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default);
    }
}
