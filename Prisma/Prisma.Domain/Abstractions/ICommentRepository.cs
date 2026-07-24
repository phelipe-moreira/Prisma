using Prisma.Domain.Entities;

namespace Prisma.Domain.Abstractions;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Comment>> GetByParentIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
    void Update(Comment comment);
}
