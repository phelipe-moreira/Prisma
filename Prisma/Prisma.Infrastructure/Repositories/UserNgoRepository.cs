using Microsoft.EntityFrameworkCore;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Enums;
using Prisma.Infrastructure.Context;

namespace Prisma.Infrastructure.Repositories;

public class UserNgoRepository(AppDbContext context) : IUserNgoRepository
{
    public Task<bool> IsAdminAsync(
        Guid userId,
        Guid ngoId,
        CancellationToken cancellationToken = default)
        => context.UserNgos.AnyAsync(
            x => x.UserId == userId &&
                 x.NgoId == ngoId &&
                 x.Role == UserNgoRole.Admin,
            cancellationToken);
}
