using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;

namespace Prisma.Application.Services;

public class NgoAccessService(IUnitOfWork unitOfWork) : INgoAccessService
{
    public Task<bool> IsAdminAsync(
        Guid userId,
        Guid ngoId,
        CancellationToken cancellationToken = default)
        => unitOfWork.UserNgoRepository.IsAdminAsync(
            userId,
            ngoId,
            cancellationToken);
}
