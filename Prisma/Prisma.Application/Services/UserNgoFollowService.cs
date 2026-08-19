using Prisma.Application.DTOs.Ngo;
using Prisma.Application.DTOs.UserNgoFollow;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services;

public class UserNgoFollowService(IUnitOfWork unitOfWork) : IUserNgoFollowService
{
    public async Task<Result<UserNgoFollowResponse>> CreateAsync(Guid userId, CreateUserNgoFollowRequest request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

        if(user is null)
            return Result<UserNgoFollowResponse>.Failure(UserNgoFollowErrors.UserNotFound);

        var ngo = await unitOfWork.NgoRepository.GetByIdAsync(request.NgoId, cancellationToken);

        if(ngo is null)
            return Result<UserNgoFollowResponse>.Failure(UserNgoFollowErrors.NgoNotFound);

        var exists = await unitOfWork.UserNgoFollowRepository.ExistsAsync(userId, request.NgoId, cancellationToken);

        if(exists)
            return Result<UserNgoFollowResponse>.Failure(UserNgoFollowErrors.AlreadyExists);

        var userNgoFollow = UserNgoFollow.Create(userId, request.NgoId);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.UserNgoFollowRepository.AddAsync(userNgoFollow, cancellationToken);

                return Result<UserNgoFollowResponse>.Success(MapToResponse(userNgoFollow, user.Name, ngo.Name));

            }, cancellationToken);

        if (transactionResult.IsFailure)
            return Result<UserNgoFollowResponse>.Failure(transactionResult.Errors);

        return Result.Success(MapToResponse(userNgoFollow, user.Name, ngo.Name));
    }

    public async Task<Result> DeleteAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken)
    {
        var follow = await unitOfWork.UserNgoFollowRepository.GetAsync(userId, ngoId, cancellationToken);

        if(follow is null)
            return Result.Failure(UserNgoFollowErrors.NotFound);

        return await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.UserNgoFollowRepository.DeleteAsync(follow, cancellationToken);

                return Result.Success();

            }, cancellationToken);
    }

    public async Task<Result<IEnumerable<UserNgoFollowResponse>>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken)
    {
        var follows = await unitOfWork.UserNgoFollowRepository.GetByNgoIdAsync(ngoId, cancellationToken);

        var response = follows.Select(x => MapToResponse(x, x.User.Name, x.Ngo.Name));

        return Result<IEnumerable<UserNgoFollowResponse>>.Success(response);
    }

    public async Task<Result<IEnumerable<UserNgoFollowResponse>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var follows = await unitOfWork.UserNgoFollowRepository.GetByUserIdAsync(userId, cancellationToken);

        var response = follows.Select(x => MapToResponse(x, x.User.Name, x.Ngo.Name));

        return Result<IEnumerable<UserNgoFollowResponse>>.Success(response);
    }

    private static UserNgoFollowResponse MapToResponse(UserNgoFollow userNgoFollow, string userName, string ngoName)
    {
        return new UserNgoFollowResponse
        {
            UserId = userNgoFollow.UserId,
            UserName = userName,
            NgoId = userNgoFollow.NgoId,
            NgoName = ngoName,
            FollowedAt = userNgoFollow.FollowedAt
        };
    }
}
