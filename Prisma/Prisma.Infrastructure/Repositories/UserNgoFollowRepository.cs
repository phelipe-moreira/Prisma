using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prisma.Infrastructure.Repositories
{
    public class UserNgoFollowRepository(AppDbContext context) : IUserNgoFollowRepository
    {
        public async Task AddAsync(UserNgoFollow userNgoFollow, CancellationToken cancellationToken)
        {
            await context.UserNgoFollows.AddAsync(userNgoFollow, cancellationToken);
        }

        public Task DeleteAsync(UserNgoFollow userNgoFollow, CancellationToken cancellationToken)
        {
            context.UserNgoFollows.Remove(userNgoFollow);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken)
        {
            return await context.UserNgoFollows
                .AnyAsync(x => x.UserId == userId && x.NgoId == ngoId, cancellationToken);
        }

        public async Task<UserNgoFollow?> GetAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken)
        {
            return await context.UserNgoFollows
                .Include(x => x.User)
                .Include(x => x.Ngo)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.NgoId == ngoId, cancellationToken);
        }

        public async Task<IEnumerable<UserNgoFollow>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken)
        {
            return await context.UserNgoFollows
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.NgoId == ngoId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserNgoFollow>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await context.UserNgoFollows
                .AsNoTracking()
                .Include(x => x.Ngo)
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
