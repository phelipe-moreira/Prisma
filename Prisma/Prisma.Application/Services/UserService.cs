using Prisma.Application.DTOs.User;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IUserService
    {
        public async Task<Result> DeactivateAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
                return Result.Failure(UserErrors.NotFound);

            if (!user.IsActive)
                return Result.Failure(UserErrors.Inactive);

            user.Deactivate();

            return await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

                    return Result.Success();

                }, cancellationToken);
        }

        public async Task<Result<UserResponse>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
                return Result<UserResponse>.Failure(UserErrors.NotFound);

            return Result<UserResponse>.Success(MapToResponse(user));
        }

        public async Task<Result> UpdatePassowdAsync(Guid userId, UpdatePasswordRequest request, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
                return Result.Failure(UserErrors.NotFound);

            if (!user.IsActive)
                return Result.Failure(UserErrors.Inactive);

            var currentPasswordIsValid = passwordHasher.Verify(request.CurrentPassword, user.PasswordHash);

            if (!currentPasswordIsValid)
                return Result.Failure(UserErrors.InvalidCurrentPassword);

            var newPasswordHash = passwordHasher.Hash(request.NewPassword);

            user.UpdatePassword(newPasswordHash);

            return await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

                    return Result.Success();
                }, cancellationToken);
        }

        public async Task<Result<UserResponse>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
                return Result<UserResponse>.Failure(UserErrors.NotFound);

            if (!user.IsActive)
                return Result<UserResponse>.Failure(UserErrors.Inactive);

            user.UpdateProfile(request.Name, request.Bio, request.ProfilePictureUrl);

            var transactionResult = await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

                    return Result.Success();

                }, cancellationToken);

            if (transactionResult.IsFailure)
                return Result<UserResponse>.Failure(transactionResult.Errors);

            return Result<UserResponse>.Success(MapToResponse(user));
        }

        private static UserResponse MapToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Bio = user.Bio,
                CreatedAt = user.CreatedAt,
                Role = user.Role
            };
        }
    }
}
