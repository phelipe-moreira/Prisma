using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Entities;
using Prisma.Domain.Interfaces;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories
{
    public class CauseRepository : ICauseRepository
    {
        private readonly AppDbContext _context;

        public CauseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Cause cause, CancellationToken cancellationToken)
        {
            await _context.Causes.AddAsync(cause, cancellationToken);
        }

        public Task DeleteAsync(Cause cause, CancellationToken cancellationToken)
        {
            _context.Causes.Remove(cause);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Causes.
                AnyAsync(
                c => c.Name == name,
                cancellationToken);
        }

        public async Task<IEnumerable<Cause>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Causes
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Cause?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Causes
            .FirstOrDefaultAsync(
                c => c.Id == id,
                cancellationToken);
        }

        public Task UpdateAsync(Cause cause, CancellationToken cancellationToken)
        {
            _context.Causes.Update(cause);

            return Task.CompletedTask;
        }
    }
}
