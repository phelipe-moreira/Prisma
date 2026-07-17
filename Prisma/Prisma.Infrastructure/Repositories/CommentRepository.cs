using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories;

public class CommentRepository(AppDbContext context) : ICommentRepository
{
    public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        await context.Comments.AddAsync(comment, cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Comments.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Comments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetByParentIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Comments.AsNoTracking().Where(x => x.ParentCommentId == id).ToListAsync(cancellationToken: cancellationToken);
    }

    public void Update(Comment comment)
    {
        context.Comments.Update(comment);
    }

    public void Remove(Comment comment)
    {
        context.Comments.Update(comment);
    }
}
