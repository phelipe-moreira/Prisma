using Prisma.Application.DTOs.User;
using Prisma.Application.Errors;
using Prisma.Application.Extensions;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IStorageService storageService) : IUserService
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
            var user = await unitOfWork.UserRepository.GetWithSavedPostsByIdAsync(userId, cancellationToken);

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

        public async Task<Result<Prisma.Application.DTOs.Media.UploadMediaResponse>> GetProfilePictureUploadUrlAsync(
            Guid userId,
            Prisma.Application.DTOs.Media.UploadMediaRequest request,
            CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
                return Result<Prisma.Application.DTOs.Media.UploadMediaResponse>.Failure(UserErrors.NotFound);

            if (!user.IsActive)
                return Result<Prisma.Application.DTOs.Media.UploadMediaResponse>.Failure(UserErrors.Inactive);

            var extension = request.ContentType switch
            {
                "image/jpeg" => "jpg",
                "image/png" => "png",
                "image/webp" => "webp",
                _ => null
            };

            if (extension is null)
                return Result<Prisma.Application.DTOs.Media.UploadMediaResponse>.Failure(MediaErrors.InvalidContentType);

            const long maxImageSize = 10L * 1024L * 1024L;
            if (request.FileSize <= 0 || request.FileSize > maxImageSize)
                return Result<Prisma.Application.DTOs.Media.UploadMediaResponse>.Failure(MediaErrors.FileTooLarge);

            var storageKey = $"profiles/{userId}/{Guid.NewGuid()}.{extension}";

            return Result<Prisma.Application.DTOs.Media.UploadMediaResponse>.Success(new Prisma.Application.DTOs.Media.UploadMediaResponse
            {
                Id = Guid.NewGuid(),
                StorageKey = storageKey,
                UploadUrl = storageService.GenerateUploadUrl(storageKey, request.ContentType, TimeSpan.FromMinutes(15)),
                PublicUrl = storageService.GetPublicUrl(storageKey)
            });
        }

        private UserResponse MapToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Bio = user.Bio,
                CreatedAt = user.CreatedAt,
                Role = user.Role,
                SavedPosts = [.. user.SavedPosts.Select(savedPost => savedPost.Post.ToResponse(storageService, user.Id))]
            };
        }
    }
}
