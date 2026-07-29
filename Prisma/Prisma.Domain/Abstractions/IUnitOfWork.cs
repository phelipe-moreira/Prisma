using Prisma.Domain.Models;

namespace Prisma.Domain.Abstractions;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }

    ICauseRepository CauseRepository { get; }

    ICommentRepository CommentRepository { get; }

    INgoRepository NgoRepository { get; }

    INgoCauseRepository NgoCauseRepository { get; }
    Task<Result<T>> ExecuteTransactionAsync<T>(Func<Task<Result<T>>> action, CancellationToken cancellationToken = default);
}

