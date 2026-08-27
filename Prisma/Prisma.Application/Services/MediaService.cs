using Prisma.Application.DTOs.Media;
using Prisma.Application.Errors;
using Prisma.Application.Extensions;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Enums;
using Prisma.Domain.Models;

namespace Prisma.Application.Services;

public class MediaService(IUnitOfWork unitOfWork, IStorageService storageService) : IMediaService
{
    private const long MaxImageSize = 10 * 1024 * 1024;
    private const long MaxVideoSize = 100 * 1024 * 1024;

    private static readonly HashSet<string> AllowedContentTypes =
    [
        "image/jpeg",
        "image/png",
        "image/webp",
        "video/mp4",
        "video/webm"
    ];

    public async Task<Result> CompleteUploadAsync(Guid postId, Guid mediaId, CompleteMediaUploadRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return Result.Failure(MediaErrors.RequestCannotBeNull);

        var post = await unitOfWork.PostRepository.GetByIdAsync(postId, cancellationToken);

        if (post is null)
            return Result.Failure(PostErrors.NotFound);

        var exists = await storageService.ExistsAsync(request.StorageKey ?? string.Empty, cancellationToken);

        if (!exists)
            return Result.Failure(MediaErrors.UploadNotFound);

        var media = Media.Create(mediaId, request.Type, request.StorageKey ?? string.Empty);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.MediaRepository.AddAsync(media, cancellationToken: cancellationToken);

                return Result.Success();
            }, cancellationToken);

        if (transactionResult.IsFailure)
            return Result<Guid>.Failure(transactionResult.Errors);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid mediaId, CancellationToken cancellationToken)
    {
        var media = await unitOfWork.MediaRepository.GetByIdAsync(mediaId, cancellationToken);

        if (media is null)
            return Result<MediaResponse>.Failure(MediaErrors.NotFound);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await storageService.DeleteAsync(media.StorageKey, cancellationToken);

                await unitOfWork.MediaRepository.Delete(media);

                return Result.Success();
            },
            cancellationToken);

        if (transactionResult.IsFailure)
            return Result.Failure(transactionResult.Errors);

        return Result.Success();
    }

    public async Task<Result<MediaResponse>> GetAsync(Guid mediaId, CancellationToken cancellationToken)
    {
        var media = await unitOfWork.MediaRepository.GetByIdAsync(mediaId, cancellationToken);

        if (media is null)
            return Result<MediaResponse>.Failure(MediaErrors.NotFound);

        var mediaReponse = media.ToResponse();
        mediaReponse.Url = storageService.GetPublicUrl(media.StorageKey);

        return Result.Success(mediaReponse);
    }

    public async Task<Result<UploadMediaResponse>> GetUploadUrlAsync(Guid postId, UploadMediaRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return Result<UploadMediaResponse>.Failure(MediaErrors.RequestCannotBeNull);

        if (!AllowedContentTypes.Contains(request.ContentType))
            return Result<UploadMediaResponse>.Failure(MediaErrors.InvalidContentType);

        var mediaType = request.ContentType.StartsWith("image/")
            ? MediaType.Image
            : MediaType.Video;

        if (mediaType is MediaType.Image && request.FileSize > MaxImageSize)
            return Result<UploadMediaResponse>.Failure(MediaErrors.FileTooLarge);

        if (mediaType is MediaType.Video && request.FileSize > MaxVideoSize)
            return Result<UploadMediaResponse>.Failure(MediaErrors.FileTooLarge);

        var post = await unitOfWork.PostRepository.GetByIdAsync(postId, cancellationToken);

        if (post is null)
            return Result<UploadMediaResponse>.Failure(PostErrors.NotFound);

        var mediaId = Guid.NewGuid();

        var extension = request.ContentType.Split('/')[1];

        var storageKey = mediaType is MediaType.Image
            ? $"images/{mediaId}.{extension}"
            : $"videos/{mediaId}.{extension}";

        var uploadUrl = storageService.GenerateUploadUrl( 
            storageKey,
            request.ContentType,
            TimeSpan.FromMinutes(15));

        var response = new UploadMediaResponse
        {
            Id = mediaId,
            StorageKey = storageKey,
            UploadUrl = uploadUrl
        };

        return Result.Success(response);
    }
}
