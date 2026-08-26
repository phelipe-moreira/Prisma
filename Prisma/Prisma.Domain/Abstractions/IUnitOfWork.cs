using Prisma.Domain.Models;

namespace Prisma.Domain.Abstractions;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }

    ICauseRepository CauseRepository { get; }

    ICommentRepository CommentRepository { get; }

    INgoRepository NgoRepository { get; }

    IUserNgoFollowRepository UserNgoFollowRepository { get; }

    IPostRepository PostRepository { get; }

    IPostLikeRepository PostLikeRepository { get; }

    ISavedPostRepository SavedPostRepository { get; }

    IPostMediaRepository PostMediaRepository { get; }

    IMediaRepository MediaRepository { get; }

    Task<Result> ExecuteTransactionAsync(Func<Task<Result>> action, CancellationToken cancellationToken = default);
}

