using Prisma.Application.Errors;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Models;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public IUserRepository UserRepository => field ??= new UserRepository(context);
    public ICauseRepository CauseRepository => field ??= new CauseRepository(context);
    public INgoRepository NgoRepository => field ??= new NgoRepository(context);

    public ICommentRepository CommentRepository => field ??= new CommentRepository(context);

    //public INgoCauseRepository NgoCauseRepository => field ??= new NgoCauseRepository(context);

    public IUserNgoRepository UserNgoRepository => field ??= new UserNgoRepository(context);

    public IUserNgoFollowRepository UserNgoFollowRepository => field ??= new UserNgoFollowRepository(context);

    public async Task<Result<T>> ExecuteTransactionAsync<T>(Func<Task<Result<T>>> action, CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await action();

            if (result.IsFailure)
            {
                await transaction.RollbackAsync(cancellationToken);
                return result;
            }

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result<T>.Failure(TransactionErrors.Failed);
        }
    }
}

