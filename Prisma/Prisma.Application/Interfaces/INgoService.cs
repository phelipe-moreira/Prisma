using Prisma.Application.DTOs.Ngo;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces
{
    public interface INgoService
    {
        Task<Result<IEnumerable<NgoResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<NgoResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<NgoResponse>> CreateAsync(CreateNgoRequest request, CancellationToken cancellationToken = default);
        Task<Result<NgoResponse>> UpdateAsync(Guid id, UpdateNgoRequest request, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
