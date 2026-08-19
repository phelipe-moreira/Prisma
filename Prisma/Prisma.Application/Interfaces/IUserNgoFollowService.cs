using Prisma.Application.DTOs.UserNgoFollow;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces
{
    public interface IUserNgoFollowService
    {
        Task<Result<UserNgoFollowResponse>> CreateAsync(CreateUserNgoFollowRequest request, CancellationToken cancellationToken);
        Task<Result<IEnumerable<UserNgoFollowResponse>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Result<IEnumerable<UserNgoFollowResponse>>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken);
    }
}
