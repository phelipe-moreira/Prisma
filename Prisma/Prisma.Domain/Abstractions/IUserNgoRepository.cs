namespace Prisma.Domain.Abstractions;

public interface IUserNgoRepository
{
    Task<bool> IsAdminAsync(
        Guid userId,
        Guid ngoId,
        CancellationToken cancellationToken = default);
}
