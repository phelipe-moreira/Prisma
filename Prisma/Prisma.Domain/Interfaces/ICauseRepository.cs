using Prisma.Domain.Entities;

namespace Prisma.Domain.Interfaces
{
    public interface ICauseRepository
    {
        Task<IEnumerable<Cause>> GetAllAsync(CancellationToken cancellationToken);
        Task<Cause?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Cause cause, CancellationToken cancellationToken);
        Task UpdateAsync(Cause cause, CancellationToken cancellationToken);
        Task DeleteAsync(Cause cause, CancellationToken cancellationToken);
    }
}
