using Prisma.Application.DTOs.Comment;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services;

public class CommentService(IUnitOfWork unitOfWork) : ICommentService
{
    public async Task<Result<Guid>> CreateAsync(Guid userId, CommentDto commentDto, CancellationToken cancellationToken = default)
    {
        if (commentDto is null)
            return Result<Guid>.Failure(CommentErros.RequestCannotBeNull);

        var comment = Comment.Create(commentDto.PostId, userId, commentDto.ParentCommentId, commentDto.Content);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.CommentRepository.AddAsync(comment, cancellationToken: cancellationToken);

                return Result.Success();
            }, cancellationToken);

        if (transactionResult.IsFailure)
            return Result<Guid>.Failure(transactionResult.Errors);

        return Result.Success(comment.Id);
    }

    public async Task<Result<IEnumerable<CommentResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var comments = await unitOfWork.CommentRepository.GetAllAsync(cancellationToken);

        return Result<IEnumerable<CommentResponse>>.Success(
            comments.Select(c => new CommentResponse(
                c.Id,
                c.PostId,
                c.UserId,
                c.ParentCommentId,
                c.Content,
                c.CreatedAt)));
    }

    public async Task<Result<CommentResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await unitOfWork.CommentRepository.GetByIdAsync(id, cancellationToken);

        if (comment is null)
            return Result<CommentResponse>.Failure(CommentErros.NotFound);

        var commentResponse = new CommentResponse(
                comment.Id,
                comment.PostId,
                comment.UserId,
                comment.ParentCommentId,
                comment.Content,
                comment.CreatedAt);

        return Result.Success(commentResponse);
    }

    public async Task<Result<IEnumerable<CommentResponse>>> GetByParentIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comments = await unitOfWork.CommentRepository.GetByParentIdAsync(id, cancellationToken);

        return Result<IEnumerable<CommentResponse>>.Success(
            comments.Select(c => new CommentResponse(
                c.Id,
                c.PostId,
                c.UserId,
                c.ParentCommentId,
                c.Content,
                c.CreatedAt)));
    }

    public async Task<Result<Guid>> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await unitOfWork.CommentRepository.GetByIdAsync(id, cancellationToken);

        if (comment is null)
            return Result<Guid>.Failure(CommentErros.NotFound);

        comment.Remove();

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                unitOfWork.CommentRepository.Update(comment);

                return Result.Success();
            }, cancellationToken);

        if (transactionResult.IsFailure)
            return Result<Guid>.Failure(transactionResult.Errors);

        return Result.Success(comment.Id);
    }

    public async Task<Result<Guid>> UpdateAsync(Guid id, UpdateCommentDto updateCommentDto, CancellationToken cancellationToken = default)
    {
        var comment = await unitOfWork.CommentRepository.GetByIdAsync(id, cancellationToken);

        if (comment is null)
            return Result<Guid>.Failure(CommentErros.NotFound);

        comment.UpdateComment(updateCommentDto.Content);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                unitOfWork.CommentRepository.Update(comment);

                return Result.Success();
            }, cancellationToken);

        if (transactionResult.IsFailure)
            return Result<Guid>.Failure(transactionResult.Errors);

        return Result.Success(comment.Id);
    }
}
