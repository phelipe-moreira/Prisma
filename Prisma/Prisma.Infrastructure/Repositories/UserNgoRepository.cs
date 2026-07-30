using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prisma.Infrastructure.Repositories
{
    public class UserNgoRepository(AppDbContext context) : IUserNgoRepository
    {
        public async Task AddAsync(UserNgo userNgo, CancellationToken cancellationToken)
        {
            await context.UserNgos.AddAsync(userNgo, cancellationToken);
        }

        public Task DeleteAsync(UserNgo userNgo, CancellationToken cancellationToken)
        {
            context.UserNgos.Remove(userNgo);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken)
        {
            return await context.UserNgos
                .AnyAsync(x => x.UserId == userId && x.NgoId == ngoId, cancellationToken);
        }

        public async Task<UserNgo?> GetAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken)
        {
            return await context.UserNgos
                .FirstOrDefaultAsync(x => x.UserId == userId && x.NgoId == ngoId, cancellationToken);
        }

        public async Task<IEnumerable<UserNgo>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken)
        {
            return await context.UserNgos
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.NgoId == ngoId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserNgo>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await context.UserNgos
                .AsNoTracking()
                .Include(x => x.Ngo)
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
