using Prisma.Domain.Models;

namespace Prisma.Domain.Abstractions;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }

    ICauseRepository CauseRepository { get; }

    ICommentRepository CommentRepository { get; }

    INgoRepository NgoRepository { get; }

    IUserNgoRepository UserNgoRepository { get; }

    IUserNgoFollowRepository UserNgoFollowRepository { get; }

    Task<Result<T>> ExecuteTransactionAsync<T>(Func<Task<Result<T>>> action, CancellationToken cancellationToken = default);
}

