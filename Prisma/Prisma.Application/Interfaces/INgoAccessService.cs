namespace Prisma.Application.Interfaces;

public interface INgoAccessService
{
    Task<bool> IsAdminAsync(
        Guid userId,
        Guid ngoId,
        CancellationToken cancellationToken = default);
}
