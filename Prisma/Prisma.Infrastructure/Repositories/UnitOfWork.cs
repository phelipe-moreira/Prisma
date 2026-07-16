using Microsoft.EntityFrameworkCore.Storage;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Interfaces;
using Prisma.Domain.Models;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public IUserRepository UserRepository => field ??= new UserRepository(context);

    public ICommentRepository CommentRepository => field ??= new CommentRepository(context);

    public async Task<Result<T>> ExecuteTransactionAsync<T>(Func<CancellationToken, Task<Result<T>>> action, CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await action(cancellationToken);

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
            throw;
        }
    }
}
