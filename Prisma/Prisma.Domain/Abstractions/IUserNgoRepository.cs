using Prisma.Domain.Entities;

namespace Prisma.Domain.Abstractions
{
    public interface IUserNgoRepository
    {
        Task AddAsync(UserNgo userNgo, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken);
        Task<UserNgo?> GetAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken);
        Task<IEnumerable<UserNgo>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<UserNgo>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken);
        Task DeleteAsync(UserNgo userNgo, CancellationToken cancellationToken);
    }
}
