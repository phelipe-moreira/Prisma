using Prisma.Application.DTOs.UserNgo;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces
{
    public interface IUserNgoService
    {
        Task<Result<UserNgoResponse>> CreateAsync(CreateUserNgoRequest request, CancellationToken cancellationToken);
        Task<Result<IEnumerable<UserNgoResponse>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Result<IEnumerable<UserNgoResponse>>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken);
        Task DeleteAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken);
    }
}
