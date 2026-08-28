using Prisma.Application.DTOs.Media;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces;

public interface IMediaService
{
    Task<Result<UploadMediaResponse>> GetUploadUrlAsync(Guid userId, Guid postId, UploadMediaRequest request, CancellationToken cancellationToken);

    Task<Result> CompleteUploadAsync(Guid userId, Guid postId, Guid mediaId, CompleteMediaUploadRequest request, CancellationToken cancellationToken);

    Task<Result<MediaResponse>> GetAsync(Guid userId, Guid mediaId, CancellationToken cancellationToken);

    Task<Result> DeleteAsync(Guid userId, Guid mediaId, CancellationToken cancellationToken);
}
