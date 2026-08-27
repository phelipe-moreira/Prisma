using Prisma.Application.DTOs.Auth;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Models;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;

namespace Prisma.Application.Services;

public class AuthService(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<Result<AuthToken>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await unitOfWork.UserRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
            return Result<AuthToken>.Failure(AuthErrors.InvalidCredentials);

        if (!user.IsActive)
            return Result<AuthToken>.Failure(UserErrors.Inactive);

        var passwordIsValid = passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!passwordIsValid)
            return Result<AuthToken>.Failure(AuthErrors.InvalidCredentials);

        return await GenerateAndPersistTokensAsync(user, cancellationToken);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var token = await unitOfWork.RefreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

        if (token is null || token.IsRevoked)
            return;

        token.Revoke();

        await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.RefreshTokenRepository.UpdateAsync(token, cancellationToken);

                return Result.Success();

            }, cancellationToken);
    }

    public async Task<Result<AuthToken>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = await unitOfWork.RefreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (refreshToken is null)
            return Result<AuthToken>.Failure(AuthErrors.InvalidRefreshToken);

        if (refreshToken.IsExpired)
            return Result<AuthToken>.Failure(AuthErrors.RefreshTokenExpired);

        if (refreshToken.IsRevoked)
            return Result<AuthToken>.Failure(AuthErrors.RefreshTokenRevoked);

        var user = await unitOfWork.UserRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);

        if (user is null)
            return Result<AuthToken>.Failure(UserErrors.NotFound);

        if (!user.IsActive)
            return Result<AuthToken>.Failure(UserErrors.Inactive);

        refreshToken.Revoke();

        return await GenerateAndPersistTokensAsync(user, cancellationToken, refreshToken);
    }

    public async Task<Result<AuthToken>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var emailAlreadyExists = await unitOfWork.UserRepository.ExistsByEmailAsync(email, cancellationToken);

        if (emailAlreadyExists)
            return Result<AuthToken>.Failure(AuthErrors.EmailAlreadyExists);

        var passwordHash = passwordHasher.Hash(request.Password);

        var user = User.Create(
            request.Name,
            email,
            passwordHash);

        return await GenerateAndPersistTokensAsync(user, cancellationToken, addUser: true);
    }

    private async Task<Result<AuthToken>> GenerateAndPersistTokensAsync(User user, CancellationToken cancellationToken, RefreshToken? revokeRefreshToken = null, bool addUser = false)
    {
        var tokenResult = jwtTokenService.GenerateTokens(user);

        var refreshToken = RefreshToken.Create(
            user.Id,
            tokenResult.RefreshToken,
            tokenResult.RefreshTokenExpiresAt);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                if (addUser)
                    await unitOfWork.UserRepository.AddAsync(user, cancellationToken);

                if(revokeRefreshToken is not null)
                    await unitOfWork.RefreshTokenRepository.UpdateAsync(revokeRefreshToken, cancellationToken);

                await unitOfWork.RefreshTokenRepository.AddAsync(refreshToken, cancellationToken);

                return Result.Success();
            }, cancellationToken);

        if (transactionResult.IsFailure)
            return Result<AuthToken>.Failure(transactionResult.Errors);

        var authToken = new AuthToken
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
            ExpiresAt = tokenResult.AccessTokenExpiresAt
        };

        return Result<AuthToken>.Success(authToken);
    }
}
