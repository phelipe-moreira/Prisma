using Prisma.Application.DTOs.Auth;
using Prisma.Domain.Models;

namespace Prisma.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthToken>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
        Task<Result<AuthToken>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
        Task<Result<AuthToken>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
        Task LogoutAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
