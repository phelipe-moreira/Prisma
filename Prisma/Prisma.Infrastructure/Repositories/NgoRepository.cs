using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories;

public class NgoRepository(AppDbContext context) : INgoRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Ngo ngo, CancellationToken cancellationToken = default)
    {
        await _context.Ngo.AddAsync(ngo, cancellationToken);
    }

    public Task DeleteAsync(Ngo ngo, CancellationToken cancellationToken = default)
    {
        _context.Ngo.Remove(ngo);

        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken cancellationToken = default)
    {
        return await _context.Ngo.
            AnyAsync(n => n.Cnpj == cnpj, cancellationToken);
    }

    public async Task<IEnumerable<Ngo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Ngo
            .AsNoTracking()
            .Include(x => x.NgoCauses)
            .ThenInclude(x => x.Cause)
            .Include(x => x.Members)
            .ThenInclude(x => x.User)
            .Include(x => x.Followers)
            .ThenInclude(x => x.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<Ngo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Ngo
            .Include(x => x.NgoCauses)
            .ThenInclude(x => x.Cause)
            .Include(x => x.Members)
            .ThenInclude(x => x.User)
            .Include (x => x.Followers)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task UpdateAsync(Ngo ngo, CancellationToken cancellationToken = default)
    {
        _context.Ngo.Update(ngo);

        return Task.CompletedTask;
    }
}
