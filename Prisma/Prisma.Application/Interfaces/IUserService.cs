using Prisma.Application.DTOs.User;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserResponse>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdatePassowdAsync(Guid userId, UpdatePasswordRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeactivateAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
