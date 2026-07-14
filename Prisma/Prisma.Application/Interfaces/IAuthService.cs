using Prisma.Application.DTOs.Auth;

namespace Prisma.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthToken> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
        Task<AuthToken> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
        Task<AuthToken> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
        Task LogoutAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
