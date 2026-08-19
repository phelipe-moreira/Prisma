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

        return await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.CommentRepository.AddAsync(comment, cancellationToken: cancellationToken);

                return Result<Guid>.Success(comment.Id);
            }, cancellationToken);
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

        return Result<CommentResponse>.Success(commentResponse);
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

    public async Task<Result<Guid>> RemoveAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await unitOfWork.CommentRepository.GetByIdAsync(id, cancellationToken);

        if (comment is null)
            return Result<Guid>.Failure(CommentErros.NotFound);

        if (comment.UserId != userId)
            return Result<Guid>.Failure(CommentErros.Unauthorized);

        comment.Remove();

        return await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                unitOfWork.CommentRepository.Update(comment);

                return Result<Guid>.Success(comment.Id);
            }, cancellationToken);
    }

    public async Task<Result<Guid>> UpdateAsync(Guid userId, Guid id, UpdateCommentDto updateCommentDto, CancellationToken cancellationToken = default)
    {
        var comment = await unitOfWork.CommentRepository.GetByIdAsync(id, cancellationToken);

        if (comment is null)
            return Result<Guid>.Failure(CommentErros.NotFound);

        if (comment.UserId != userId)
            return Result<Guid>.Failure(CommentErros.Unauthorized);

        comment.UpdateComment(updateCommentDto.Content);

        return await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                unitOfWork.CommentRepository.Update(comment);

                return Result<Guid>.Success(comment.Id);
            }, cancellationToken);
    }
}
