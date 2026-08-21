using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces
{
    public interface ISavedPostService
    {
        Task<Result> SaveAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default);
        Task<Result> UnsaveAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default);
    }
}
