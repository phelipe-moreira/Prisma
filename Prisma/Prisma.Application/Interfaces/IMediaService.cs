using Prisma.Application.DTOs.Media;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces;

public interface IMediaService
{
    Task<Result<UploadMediaResponse>> GetUploadUrlAsync(Guid postId, UploadMediaRequest request, CancellationToken cancellationToken);

    Task<Result> CompleteUploadAsync(Guid postId, Guid mediaId, CompleteMediaUploadRequest request, CancellationToken cancellationToken);

    Task<Result<MediaResponse>> GetAsync(Guid mediaId, CancellationToken cancellationToken);

    Task<Result> DeleteAsync(Guid mediaId, CancellationToken cancellationToken);
}
