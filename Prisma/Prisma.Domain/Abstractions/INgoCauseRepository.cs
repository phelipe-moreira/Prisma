using Prisma.Domain.Entities;

namespace Prisma.Domain.Abstractions
{
    public interface INgoCauseRepository
    {
        Task<IEnumerable<NgoCause>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken);
        Task<IEnumerable<NgoCause>> GetByCauseIdAsync(Guid causeId, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid ngoId, Guid causeId, CancellationToken cancellationToken);
        Task AddAsync(NgoCause ngoCause, CancellationToken cancellationToken);
        Task DeleteAsync(NgoCause ngoCause, CancellationToken cancellationToken);
        Task<NgoCause?> GetAsync(Guid ngoId, Guid causeId, CancellationToken cancellationToken);
    }
}
