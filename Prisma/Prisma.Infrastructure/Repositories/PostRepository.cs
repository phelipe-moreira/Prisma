using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories;

public class PostRepository(AppDbContext context) : IPostRepository
{
    public async Task AddAsync(Post post, CancellationToken cancellationToken = default)
    {
        await context.Posts.AddAsync(post, cancellationToken);
    }

    public Task DeleteAsync(Post post, CancellationToken cancellationToken = default)
    {
        context.Posts.Remove(post);

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Post>> GetAllAsync(CancellationToken cancellationToken = default)
    {
       return await context.Posts
            .AsNoTracking()
            .Include(x => x.Comments)
            .Include(x => x.PostLikes)
            .Include(x => x.SavedPosts)
            .Include(x => x.Ngo)            
            .ToListAsync(cancellationToken);
    }

    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Posts
            .AsNoTracking()
            .Include(x => x.Comments)
            .Include(x => x.PostLikes)
            .Include(x => x.SavedPosts)
            .Include(x => x.Ngo)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task UpdateAsync(Post post, CancellationToken cancellationToken = default)
    {
        context.Update(post);

        return Task.CompletedTask;
    }
}
