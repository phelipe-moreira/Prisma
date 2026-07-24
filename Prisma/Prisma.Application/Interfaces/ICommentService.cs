using Prisma.Application.DTOs.Comment;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces;

public interface ICommentService
{
    Task<Result<Guid>> CreateAsync(CommentDto commentDto, CancellationToken cancellationToken = default);
    Task<Result<Guid>> UpdateAsync(Guid id, UpdateCommentDto updateCommentDto, CancellationToken cancellationToken = default);
    Task<Result<Guid>> RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CommentResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<CommentResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CommentResponse>>> GetByParentIdAsync(Guid id, CancellationToken cancellationToken = default);
}
