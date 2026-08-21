using Prisma.Application.DTOs.Cause;
using Prisma.Application.DTOs.Post;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services;

public class PostMediaService(IUnitOfWork unitOfWork) : IPostMediaService
{

    public async Task<Result> AddAsync(Guid postId, PostMediaDto postMediaDto, CancellationToken cancellationToken)
    {
        var post = await unitOfWork.PostRepository
            .GetByIdAsync(postId, cancellationToken);

        if (post is null)
            return Result.Failure(PostErrors.NotFound);

        var media = await unitOfWork.MediaRepository
            .GetByIdAsync(postMediaDto.MediaId, cancellationToken);

        if (media is null)
            return Result.Failure(MediaErrors.NotFound);

        var alreadyExists = await unitOfWork.PostMediaRepository
            .ExistsAsync(post.Id, media.Id, cancellationToken);

        if (alreadyExists)
            return Result.Failure(PostMediaErrors.AlreadyExists);

        var postMedia = PostMedia.Create(post.Id, media.Id, postMediaDto.DisplayOrder);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.PostMediaRepository.AddAsync(postMedia, cancellationToken);

                return Result.Success();
            },
            cancellationToken);

        if (transactionResult.IsFailure)
            return Result<CauseResponse>.Failure(transactionResult.Errors);

        return Result.Success();
    }

    public async Task<Result> RemoveAsync(Guid postId, Guid mediaId, CancellationToken cancellationToken)
    {
        var postMedia = await unitOfWork.PostMediaRepository
            .GetAsync(postId, mediaId, cancellationToken);

        if (postMedia is null)
            return Result.Failure(PostMediaErrors.NotFound);

        return await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.PostMediaRepository.DeleteAsync(postMedia, cancellationToken);

                return Result.Success();
            },
            cancellationToken);
    }
}
