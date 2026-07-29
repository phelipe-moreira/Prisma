using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prisma.Infrastructure.Repositories
{
    public class NgoCauseRepository(AppDbContext context) : INgoCauseRepository
    {
        public async Task AddAsync(NgoCause ngoCause, CancellationToken cancellationToken)
        {
            await context.NgoCauses.AddAsync(ngoCause, cancellationToken);
        }

        public Task DeleteAsync(NgoCause ngoCause, CancellationToken cancellationToken)
        {
            context.NgoCauses.Remove(ngoCause);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid ngoId, Guid causeId, CancellationToken cancellationToken)
        {
            return await context.NgoCauses
                .AnyAsync(x => x.NgoId == ngoId && x.CauseId == causeId, cancellationToken);
        }

        public async Task<NgoCause?> GetAsync(Guid ngoId, Guid causeId, CancellationToken cancellationToken)
        {
            return await context.NgoCauses
                .FirstOrDefaultAsync(x => x.NgoId == ngoId && x.CauseId == causeId, cancellationToken);
        }

        public async Task<IEnumerable<NgoCause>> GetByCauseIdAsync(Guid causeId, CancellationToken cancellationToken)
        {
            return await context.NgoCauses
                .AsNoTracking()
                .Where(x => x.CauseId == causeId)
                .Include(x => x.Ngo)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<NgoCause>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken)
        {
            return await context.NgoCauses
                .AsNoTracking()
                .Where(x => x.NgoId == ngoId)
                .Include(x => x.Cause)
                .ToListAsync(cancellationToken);
        }
    }
}
