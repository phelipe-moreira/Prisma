using Prisma.Domain.Entities;

namespace Prisma.Domain.Abstractions
{
    public interface IUserNgoFollowRepository
    {
        Task AddAsync(UserNgoFollow userNgoFollow, CancellationToken cancellationToken);
        Task DeleteAsync(UserNgoFollow userNgoFollow, CancellationToken cancellationToken);
        Task<UserNgoFollow?> GetAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken);
        Task<IEnumerable<UserNgoFollow>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<UserNgoFollow>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken);
    }
}
