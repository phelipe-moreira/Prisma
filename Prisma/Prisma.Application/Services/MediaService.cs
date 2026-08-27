using Prisma.Application.DTOs.Media;
using Prisma.Application.Errors;
using Prisma.Application.Extensions;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Enums;
using Prisma.Domain.Models;

namespace Prisma.Application.Services;

public class MediaService(
    IUnitOfWork unitOfWork,
    IStorageService storageService,
    INgoAccessService ngoAccessService) : IMediaService
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

    public async Task<Result> CompleteUploadAsync(
        Guid userId,
        Guid postId,
        Guid mediaId,
        CompleteMediaUploadRequest request,
        CancellationToken cancellationToken)
    {
        var post = await unitOfWork.PostRepository.GetByIdAsync(postId, cancellationToken);

        if (post is null)
            return Result.Failure(PostErrors.NotFound);

        if (!await ngoAccessService.IsAdminAsync(userId, post.NgoId, cancellationToken))
            return Result.Failure(PostErrors.Forbidden);

        var exists = await storageService.ExistsAsync(request.StorageKey ?? string.Empty, cancellationToken);

        if (!exists)
            return Result.Failure(MediaErrors.UploadNotFound);

        var expectedPrefix = request.Type == MediaType.Image
            ? "images/"
            : "videos/";

        var storageKey = request.StorageKey;

        if (storageKey is null ||
            !storageKey.StartsWith(expectedPrefix) ||
            !storageKey.Contains(mediaId.ToString()))
            return Result.Failure(MediaErrors.InvalidStorageKey);

        var media = Media.Create(mediaId, request.Type, request.StorageKey ?? string.Empty);
        var postMedia = PostMedia.Create(postId, mediaId, request.DisplayOrder);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.MediaRepository.AddAsync(media, cancellationToken: cancellationToken);

                await unitOfWork.PostMediaRepository.AddAsync(postMedia, cancellationToken: cancellationToken);

                return Result.Success();
            }, cancellationToken);

        if (transactionResult.IsFailure)
            return Result.Failure(transactionResult.Errors);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(
        Guid userId,
        Guid mediaId,
        CancellationToken cancellationToken)
    {
        var media = await unitOfWork.MediaRepository.GetByIdAsync(mediaId, cancellationToken);

        if (media is null)
            return Result<MediaResponse>.Failure(MediaErrors.NotFound);

        if (!await CanManageMediaAsync(userId, mediaId, cancellationToken))
            return Result.Failure(PostErrors.Forbidden);

        await storageService.DeleteAsync(media.StorageKey, cancellationToken);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.MediaRepository.Delete(media);

                return Result.Success();
            },
            cancellationToken);

        if (transactionResult.IsFailure)
            return Result.Failure(transactionResult.Errors);

        return Result.Success();
    }

    public async Task<Result<MediaResponse>> GetAsync(
        Guid userId,
        Guid mediaId,
        CancellationToken cancellationToken)
    {
        var media = await unitOfWork.MediaRepository.GetByIdAsync(mediaId, cancellationToken);

        if (media is null)
            return Result<MediaResponse>.Failure(MediaErrors.NotFound);

        if (!await CanManageMediaAsync(userId, mediaId, cancellationToken))
            return Result<MediaResponse>.Failure(PostErrors.Forbidden);

        var mediaResponse = media.ToResponse();
        mediaResponse.Url = storageService.GetPublicUrl(media.StorageKey);

        return Result.Success(mediaResponse);
    }

    public async Task<Result<UploadMediaResponse>> GetUploadUrlAsync(
        Guid userId,
        Guid postId,
        UploadMediaRequest request,
        CancellationToken cancellationToken)
    {
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

        if (!await ngoAccessService.IsAdminAsync(userId, post.NgoId, cancellationToken))
            return Result<UploadMediaResponse>.Failure(PostErrors.Forbidden);

        var mediaId = Guid.NewGuid();

        var extension = GetExtension(request.ContentType);

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

    private static string GetExtension(string contentType)
    {
        return contentType switch
        {
            "image/jpeg" => "jpg",
            "image/png" => "png",
            "image/webp" => "webp",
            "video/mp4" => "mp4",
            "video/webm" => "webm",
            _ => throw new ArgumentOutOfRangeException(nameof(contentType))
        };
    }

    private async Task<bool> CanManageMediaAsync(
        Guid userId,
        Guid mediaId,
        CancellationToken cancellationToken)
    {
        var postIds = await unitOfWork.PostMediaRepository
            .GetPostIdsByMediaIdAsync(mediaId, cancellationToken);

        if (postIds.Count == 0)
            return false;

        foreach (var postId in postIds)
        {
            var post = await unitOfWork.PostRepository.GetByIdAsync(postId, cancellationToken);

            if (post is null ||
                !await ngoAccessService.IsAdminAsync(userId, post.NgoId, cancellationToken))
                return false;
        }

        return true;
    }
}
