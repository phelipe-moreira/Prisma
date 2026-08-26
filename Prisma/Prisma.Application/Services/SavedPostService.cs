using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services
{
    public class SavedPostService(IUnitOfWork unitOfWork) : ISavedPostService
    {
        public async Task<Result> UnsaveAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
        {
            var savedPost = await unitOfWork.SavedPostRepository.GetAsync(userId, postId, cancellationToken);

            if (savedPost is null)
                return Result.Failure(SavedPostErrors.NotFound);

            var transactionResult = await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.SavedPostRepository.DeleteAsync(savedPost, cancellationToken);

                    return Result.Success();

                }, cancellationToken);

            if (transactionResult.IsFailure)
                return Result.Failure(transactionResult.Errors);

            return Result.Success();
        }

        public async Task<Result> SaveAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
        {
            var post = await unitOfWork.PostRepository.GetByIdAsync(postId, cancellationToken);

            if (post is null)
                return Result.Failure(SavedPostErrors.PostNotFound);

            var alreadySaved = await unitOfWork.SavedPostRepository.ExistsAsync(userId, postId, cancellationToken);

            if (alreadySaved)
                return Result.Failure(SavedPostErrors.AlreadySaved);

            var savedPost = SavedPost.Create(userId, postId);

            var transactionResult = await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.SavedPostRepository.AddAsync(savedPost, cancellationToken);

                    return Result.Success();

                }, cancellationToken);

            if (transactionResult.IsFailure)
                return Result.Failure(transactionResult.Errors);

            return Result.Success();
        }
    }
}
