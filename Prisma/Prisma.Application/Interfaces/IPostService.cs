using Prisma.Application.DTOs.Post;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces;

public interface IPostService
{
    Task<Result<Guid>> CreateAsync(Guid userId, CreatePostDto createPostDto, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Guid userId, Guid id, UpdatePostDto updateCommentDto, CancellationToken cancellationToken = default);
    Task<Result> RemoveAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PostResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<PostResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
