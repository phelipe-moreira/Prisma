using Prisma.Application.DTOs.NgoCause;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces
{
    public interface INgoCauseService
    {
        Task<Result<NgoCauseResponse>> CreateAsync(CreateNgoCauseRequest request, CancellationToken cancellationToken);
        Task<Result<IEnumerable<NgoCauseResponse>>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken);
        Task<Result<IEnumerable<NgoCauseResponse>>> GetByCauseIdAsync(Guid causeId, CancellationToken cancellationToken);
        Task DeleteAsync(Guid ngoId, Guid causeId, CancellationToken cancellationToken);
    }
}
