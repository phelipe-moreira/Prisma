using Prisma.Application.DTOs.Cause;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces
{
    public interface ICauseService
    {
        Task<Result<IEnumerable<CauseResponse>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<CauseResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<CauseResponse>> CreateAsync(CreateCauseRequest request, CancellationToken cancellationToken);
        Task<Result<CauseResponse>> UpdateAsync(Guid id, UpdateCauseRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
