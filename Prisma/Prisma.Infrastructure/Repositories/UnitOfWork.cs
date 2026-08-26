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
    public IUserNgoFollowRepository UserNgoFollowRepository => field ??= new UserNgoFollowRepository(context);
    public IPostRepository PostRepository => field ??= new PostRepository(context);
    public IPostLikeRepository PostLikeRepository => field ??= new PostLikeRepository(context);
    public ISavedPostRepository SavedPostRepository => field ??= new SavedPostRepository(context);
    public IPostMediaRepository PostMediaRepository => field ??= new PostMediaRepository(context);
    public IMediaRepository MediaRepository => field ??= new MediaRepository(context);

    public async Task<Result> ExecuteTransactionAsync(Func<Task<Result>> action, CancellationToken cancellationToken = default)
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
            return Result.Failure(TransactionErrors.Failed);
        }
    }
}

