using Prisma.Application.DTOs.Post;
using Prisma.Application.Errors;
using Prisma.Application.Extensions;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services;

public class PostService(
    IUnitOfWork unitOfWork,
    INgoAccessService ngoAccessService) : IPostService
{
    public async Task<Result<Guid>> CreateAsync(
        Guid userId,
        CreatePostDto createPostDto,
        CancellationToken cancellationToken = default)
    {
        if (createPostDto is null)
            return Result<Guid>.Failure(PostErrors.RequestCannotBeNull);

        if (!await VerifyIfNgoExists(createPostDto.NgoId))
            return Result<Guid>.Failure(NgoErrors.NotFound);

        if (!await ngoAccessService.IsAdminAsync(userId, createPostDto.NgoId, cancellationToken))
            return Result<Guid>.Failure(PostErrors.Forbidden);

        var post = Post.Create(createPostDto.NgoId, createPostDto.Title, createPostDto.Content, createPostDto.Status);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.PostRepository.AddAsync(post, cancellationToken: cancellationToken);

                return Result.Success();
            }, cancellationToken);

        if (transactionResult.IsFailure)
            return Result<Guid>.Failure(transactionResult.Errors);

        return Result.Success(post.Id);
    }

    private async Task<bool> VerifyIfNgoExists(Guid ngoId) 
        => await unitOfWork.NgoRepository.GetByIdAsync(ngoId) is not null;

    public async Task<Result<IEnumerable<PostResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var posts = await unitOfWork.PostRepository.GetAllAsync(cancellationToken);

        return Result.Success(posts.Select(x => x.ToResponse()));
    }

    public async Task<Result<PostResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var post = await unitOfWork.PostRepository.GetByIdAsync(id, cancellationToken);

        if (post is null)
            return Result<PostResponse>.Failure(PostErrors.NotFound);

        return Result.Success(post.ToResponse());
    }

    public async Task<Result> RemoveAsync(
        Guid userId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var post = await unitOfWork.PostRepository.GetByIdAsync(id, cancellationToken);

        if (post is null)
            return Result.Failure(PostErrors.NotFound);

        if (!await ngoAccessService.IsAdminAsync(userId, post.NgoId, cancellationToken))
            return Result.Failure(PostErrors.Forbidden);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.PostRepository.DeleteAsync(post);

                return Result.Success();
            }, cancellationToken);

        if (transactionResult.IsFailure)
            return Result.Failure(transactionResult.Errors);

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(
        Guid userId,
        Guid id,
        UpdatePostDto updateCommentDto,
        CancellationToken cancellationToken = default)
    {
        if (updateCommentDto is null)
            return Result.Failure(PostErrors.RequestCannotBeNull);

        var post = await unitOfWork.PostRepository.GetByIdAsync(id, cancellationToken);

        if (post is null)
            return Result.Failure(PostErrors.NotFound);

        if (!await ngoAccessService.IsAdminAsync(userId, post.NgoId, cancellationToken))
            return Result.Failure(PostErrors.Forbidden);

        post.Update(
            updateCommentDto.Title, 
            updateCommentDto.Content, 
            updateCommentDto.Status);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.PostRepository.UpdateAsync(post, cancellationToken: cancellationToken);

                return Result.Success();
            }, cancellationToken);

        if (transactionResult.IsFailure)
            return Result.Failure(transactionResult.Errors);

        return Result.Success();
    }
}
