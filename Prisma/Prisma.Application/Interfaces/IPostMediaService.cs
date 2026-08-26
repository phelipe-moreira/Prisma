using Prisma.Application.DTOs.Post;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces;

public interface IPostMediaService
{
    Task<Result> AddAsync(Guid postId, PostMediaDto postMediaDto, CancellationToken cancellationToken);

    Task<Result> RemoveAsync(Guid postId, Guid mediaId, CancellationToken cancellationToken);
}
