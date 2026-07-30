using Prisma.Application.DTOs.UserNgo;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services
{
    public class UserNgoService(IUnitOfWork unitOfWork) : IUserNgoService
    {
        public async Task<Result<UserNgoResponse>> CreateAsync(CreateUserNgoRequest request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.UserId, cancellationToken);

            if(user is null)
                return Result<UserNgoResponse>.Failure(UserNgoErrors.UserNotFound);

            var ngo = await unitOfWork.NgoRepository.GetByIdAsync(request.NgoId, cancellationToken);

            if(ngo is null)
                return Result<UserNgoResponse>.Failure(UserNgoErrors.NgoNotFound);

            var exists = await unitOfWork.UserNgoRepository.ExistsAsync(request.UserId, request.NgoId, cancellationToken);

            if (exists)
                return Result<UserNgoResponse>.Failure(UserNgoErrors.AlreadyExists);

            return await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    var userNgo = UserNgo.Create(request.UserId,request.NgoId, request.Role);

                    await unitOfWork.UserNgoRepository.AddAsync(userNgo, cancellationToken);

                    var response = MapToResponse(userNgo, user.Name, ngo.Name);

                    return Result<UserNgoResponse>.Success(response);
                },
                cancellationToken);
        }

        public async Task DeleteAsync(Guid userId, Guid ngoId, CancellationToken cancellationToken)
        {
            var userNgo = await unitOfWork.UserNgoRepository.GetAsync(userId, ngoId, cancellationToken);

            if (userNgo is null)
                return;

            await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.UserNgoRepository.DeleteAsync(userNgo, cancellationToken);

                    return Result<bool>.Success(true);

                }, cancellationToken);
        }

        public async Task<Result<IEnumerable<UserNgoResponse>>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken)
        {
            var userNgos = await unitOfWork.UserNgoRepository.GetByNgoIdAsync(ngoId, cancellationToken);

            var response = userNgos.Select(x => MapToResponse(x, x.User.Name, x.Ngo.Name));

            return Result<IEnumerable<UserNgoResponse>>.Success(response);
        }

        public async Task<Result<IEnumerable<UserNgoResponse>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var userNgos = await unitOfWork.UserNgoRepository.GetByUserIdAsync(userId, cancellationToken);

            var response = userNgos.Select(x => MapToResponse(x, x.User.Name, x.Ngo.Name));

            return Result<IEnumerable<UserNgoResponse>>.Success(response);
        }

        private static UserNgoResponse MapToResponse(UserNgo userNgo, string userName, string ngoName)
        {
            return new UserNgoResponse
            {
                UserId = userNgo.UserId,
                UserName = userName,
                NgoId = userNgo.NgoId,
                NgoName = ngoName,
                Role = userNgo.Role,
                CreatedAt = userNgo.CreatedAt
            };
        }
    }
}
