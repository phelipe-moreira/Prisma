using Prisma.Application.DTOs.Auth;
using Prisma.Application.Interfaces;
using Prisma.Domain.Entities;
using Prisma.Domain.Interfaces;
using Prisma.Domain.Interfaces.Security;

namespace Prisma.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            IUserRepository userRepository, 
            IRefreshTokenRepository refreshTokenRepository, 
            IPasswordHasher passwordHasher, 
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthToken> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null)
                throw new InvalidOperationException("E-mail ou senha inválidos.");

            if (!user.IsActive)
                throw new InvalidOperationException("Usuário desativado.");

            var passwordIsValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!passwordIsValid)
                throw new InvalidOperationException("E-mail ou senha inválidos.");

            return await GenerateAndPersistTokensAsync(user, cancellationToken);
        }

        public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

            if (token is null)
                return;

            if (token.IsRevoked)
                return;

            token.Revoke();

            await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
        }

        public async Task<AuthToken> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

            if (refreshToken is null)
                throw new InvalidOperationException("Refresh Token inválido.");

            if (refreshToken.IsExpired)
                throw new InvalidOperationException("Refresh Token expirado.");

            if (refreshToken.IsRevoked)
                throw new InvalidOperationException("Refresh Token revogado.");

            var user = await _userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);

            if (user is null)
                throw new InvalidOperationException("Usuário não encontrado.");

            if (!user.IsActive)
                throw new InvalidOperationException("Usuário desativado.");

            refreshToken.Revoke();

            await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

            return await GenerateAndPersistTokensAsync(user, cancellationToken);
        }

        public async Task<AuthToken> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            var emailAlreadyExists =
                await _userRepository.ExistsByEmailAsync(
                    request.Email, 
                    cancellationToken);

            if (emailAlreadyExists)
                throw new InvalidOperationException("Já existe um usuário com este e-mail.");

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = User.Create(
                request.Name,
                request.Email,
                passwordHash);

            await _userRepository.AddAsync(user, cancellationToken);

            return await GenerateAndPersistTokensAsync(user, cancellationToken);
        }

        private async Task<AuthToken> GenerateAndPersistTokensAsync(User user, CancellationToken cancellationToken)
        {
            var tokenResult = _jwtTokenService.GenerateTokens(user);

            var refreshToken = RefreshToken.Create(
                user.Id,
                tokenResult.RefreshToken,
                tokenResult.RefreshTokenExpiresAt);

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            return new AuthToken
            {
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                ExpiresAt = tokenResult.AccessTokenExpiresAt
            };
        }
    }
}
