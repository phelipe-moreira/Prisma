namespace Prisma.Application.Interfaces;

public interface IStorageService
{
    string GenerateUploadUrl(string storageKey, string contentType, TimeSpan expiration);

    string GetPublicUrl(string storageKey);

    Task<bool> ExistsAsync(string storageKey, CancellationToken cancellationToken);

    Task DeleteAsync(string storageKey, CancellationToken cancellationToken);
}
