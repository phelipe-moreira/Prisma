using Prisma.Application.DTOs.Auth;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Application.Results;
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

        public async Task<Result<AuthToken>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null)
                return Result<AuthToken>.Failure(AuthErrors.InvalidCredentials);

            if (!user.IsActive)
                return Result<AuthToken>.Failure(AuthErrors.UserInactive);

            var passwordIsValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!passwordIsValid)
                return Result<AuthToken>.Failure(AuthErrors.InvalidCredentials);

            return await GenerateAndPersistTokensAsync(user, cancellationToken);
        }

        public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

            if (token is null || token.IsRevoked)
                return;

            token.Revoke();

            await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
        }

        public async Task<Result<AuthToken>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

            if (refreshToken is null)
                return Result<AuthToken>.Failure(AuthErrors.InvalidRefreshToken);

            if (refreshToken.IsExpired)
                return Result<AuthToken>.Failure(AuthErrors.RefreshTokenExpired);

            if (refreshToken.IsRevoked)
                return Result<AuthToken>.Failure(AuthErrors.RefreshTokenRevoked);

            var user = await _userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);

            if (user is null)
                return Result<AuthToken>.Failure(AuthErrors.UserNotFound);

            if (!user.IsActive)
                return Result<AuthToken>.Failure(AuthErrors.UserInactive);

            refreshToken.Revoke();

            await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

            return await GenerateAndPersistTokensAsync(user, cancellationToken);
        }

        public async Task<Result<AuthToken>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            var emailAlreadyExists =
                await _userRepository.ExistsByEmailAsync(
                    request.Email, 
                    cancellationToken);

            if (emailAlreadyExists)
                return Result<AuthToken>.Failure(AuthErrors.EmailAlreadyExists);

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = User.Create(
                request.Name,
                request.Email,
                passwordHash);

            await _userRepository.AddAsync(user, cancellationToken);

            return await GenerateAndPersistTokensAsync(user, cancellationToken);
        }

        private async Task<Result<AuthToken>> GenerateAndPersistTokensAsync(User user, CancellationToken cancellationToken)
        {
            var tokenResult = _jwtTokenService.GenerateTokens(user);

            var refreshToken = RefreshToken.Create(
                user.Id,
                tokenResult.RefreshToken,
                tokenResult.RefreshTokenExpiresAt);

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            var authToken = new AuthToken
            {
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                ExpiresAt = tokenResult.AccessTokenExpiresAt
            };

            return Result<AuthToken>.Success(authToken);
        }
    }
}
