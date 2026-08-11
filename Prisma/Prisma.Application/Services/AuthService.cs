using Prisma.Application.DTOs.Auth;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Models;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;

namespace Prisma.Application.Services;

public class AuthService(
    IUnitOfWork unitOfWork,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<Result<AuthToken>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return Result<AuthToken>.Failure(AuthErrors.InvalidCredentials);

        if (!user.IsActive)
            return Result<AuthToken>.Failure(AuthErrors.UserInactive);

        var passwordIsValid = passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!passwordIsValid)
            return Result<AuthToken>.Failure(AuthErrors.InvalidCredentials);

        return await GenerateAndPersistTokensAsync(user, cancellationToken);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var token = await refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

        if (token is null || token.IsRevoked)
            return;

        token.Revoke();

        await refreshTokenRepository.UpdateAsync(token, cancellationToken);
    }

    public async Task<Result<AuthToken>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (refreshToken is null)
            return Result<AuthToken>.Failure(AuthErrors.InvalidRefreshToken);

        if (refreshToken.IsExpired)
            return Result<AuthToken>.Failure(AuthErrors.RefreshTokenExpired);

        if (refreshToken.IsRevoked)
            return Result<AuthToken>.Failure(AuthErrors.RefreshTokenRevoked);

        var user = await unitOfWork.UserRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);

        if (user is null)
            return Result<AuthToken>.Failure(AuthErrors.UserNotFound);

        if (!user.IsActive)
            return Result<AuthToken>.Failure(AuthErrors.UserInactive);

        refreshToken.Revoke();

        await refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

        return await GenerateAndPersistTokensAsync(user, cancellationToken);
    }

    public async Task<Result<AuthToken>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var emailAlreadyExists =
            await unitOfWork.UserRepository.ExistsByEmailAsync(
                request.Email, 
                cancellationToken);

        if (emailAlreadyExists)
            return Result<AuthToken>.Failure(AuthErrors.EmailAlreadyExists);

        var passwordHash = passwordHasher.Hash(request.Password);

        var user = User.Create(
            request.Name,
            request.Email,
            passwordHash);

        await unitOfWork.UserRepository.AddAsync(user, cancellationToken);

        return await GenerateAndPersistTokensAsync(user, cancellationToken);
    }

    private async Task<Result<AuthToken>> GenerateAndPersistTokensAsync(User user, CancellationToken cancellationToken)
    {
        var tokenResult = jwtTokenService.GenerateTokens(user);

        var refreshToken = RefreshToken.Create(
            user.Id,
            tokenResult.RefreshToken,
            tokenResult.RefreshTokenExpiresAt);

        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        var authToken = new AuthToken
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
            ExpiresAt = tokenResult.AccessTokenExpiresAt
        };

        return Result<AuthToken>.Success(authToken);
    }
}
