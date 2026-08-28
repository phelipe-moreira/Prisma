using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(
                    x => x.Token == token,
                    cancellationToken);
        }

        public Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            _context.RefreshTokens.Update(refreshToken);

            return Task.CompletedTask;
        }
    }
}
