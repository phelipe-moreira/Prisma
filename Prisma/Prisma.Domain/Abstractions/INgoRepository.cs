using Prisma.Domain.Entities;

namespace Prisma.Domain.Abstractions
{
    public interface INgoRepository
    {
        Task<IEnumerable<Ngo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Ngo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken cancellationToken = default);
        Task AddAsync(Ngo ngo, CancellationToken cancellationToken = default);
        Task UpdateAsync(Ngo ngo, CancellationToken cancellationToken = default);
        Task DeleteAsync(Ngo ngo, CancellationToken cancellationToken = default);
    }
}
