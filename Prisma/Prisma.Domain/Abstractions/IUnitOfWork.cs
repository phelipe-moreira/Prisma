using Prisma.Domain.Interfaces;
using Prisma.Domain.Models;

namespace Prisma.Domain.Abstractions;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }

    Task<Result<T>> ExecuteTransactionAsync<T>(Func<CancellationToken, Task<Result<T>>> action, CancellationToken cancellationToken = default);
}
