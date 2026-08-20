using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services
{
    public class PostLikeService(IUnitOfWork unitOfWork) : IPostLikeService
    {
        public async Task<Result> LikeAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
        {
            var post = await unitOfWork.PostRepository.GetByIdAsync(postId, cancellationToken);

            if (post is null)
                return Result.Failure(PostLikeErrors.PostNotFound);

            var alreadyLiked = await unitOfWork.PostLikeRepository.ExistsAsync(userId, postId, cancellationToken);

            if (alreadyLiked)
                return Result.Failure(PostLikeErrors.AlreadyLiked);

            var postLike = PostLike.Create(userId, postId);

            var transactionResult = await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.PostLikeRepository.AddAsync(postLike, cancellationToken);

                    return Result.Success();
                },
                cancellationToken);

            if (transactionResult.IsFailure)
                return Result.Failure(transactionResult.Errors);

            return Result.Success();
        }

        public async Task<Result> UnlikeAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
        {
            var post = await unitOfWork.PostRepository.GetByIdAsync(postId, cancellationToken);

            if (post is null)
                return Result.Failure(PostLikeErrors.PostNotFound);

            var postLike = await unitOfWork.PostLikeRepository.GetAsync(userId, postId, cancellationToken);

            if (postLike is null)
                return Result.Failure(PostLikeErrors.NotLiked);

            var transactionResult =  await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.PostLikeRepository.DeleteAsync(postLike, cancellationToken);

                    return Result.Success();
                },
                cancellationToken);

            if (transactionResult.IsFailure)
                return Result.Failure(transactionResult.Errors);

            return Result.Success();
        }
    }
}
