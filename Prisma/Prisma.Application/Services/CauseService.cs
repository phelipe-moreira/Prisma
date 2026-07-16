using Prisma.Application.DTOs.Cause;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services
{
    public class CauseService(IUnitOfWork unitOfWork) : ICauseService
    {
        public async Task<Result<CauseResponse>> CreateAsync(CreateCauseRequest request, CancellationToken cancellationToken)
        {
            var exists = await unitOfWork.CauseRepository.ExistsByNameAsync(request.Name, cancellationToken);

            if (exists)
                return Result<CauseResponse>.Failure(CauseErrors.AlreadyExists);

            return await unitOfWork.ExecuteTransactionAsync(
                async ct =>
                {
                    var cause = Cause.Create(request.Name,request.Description);

                    await unitOfWork.CauseRepository.AddAsync(cause, ct);

                    return Result<CauseResponse>.Success(MapToResponse(cause));
                },
            cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var cause = await unitOfWork.CauseRepository.GetByIdAsync(id, cancellationToken);

            if (cause is null)
                return;

            await unitOfWork.ExecuteTransactionAsync(
                async ct =>
                {
                    await unitOfWork.CauseRepository.DeleteAsync(cause, ct);

                    return Result<bool>.Success(true);
                },
            cancellationToken);
        }

        public async Task<Result<IEnumerable<CauseResponse>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var causes = await unitOfWork.CauseRepository.GetAllAsync(cancellationToken);

            var response = causes.Select(MapToResponse);

            return Result<IEnumerable<CauseResponse>>.Success(response);
        }

        public async Task<Result<CauseResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var cause = await unitOfWork.CauseRepository.GetByIdAsync(id, cancellationToken);

            if (cause is null)
                return Result<CauseResponse>.Failure(CauseErrors.NotFound);

            return Result<CauseResponse>.Success(MapToResponse(cause));
        }

        public async Task<Result<CauseResponse>> UpdateAsync(Guid id, UpdateCauseRequest request, CancellationToken cancellationToken)
        {
            var cause = await unitOfWork.CauseRepository.GetByIdAsync(id, cancellationToken);

            if (cause is null)
                return Result<CauseResponse>.Failure(CauseErrors.NotFound);

            return await unitOfWork.ExecuteTransactionAsync(
                async ct =>
                {
                    cause.Update(request.Name,request.Description);

                    await unitOfWork.CauseRepository.UpdateAsync(cause, ct);

                    return Result<CauseResponse>.Success(MapToResponse(cause));
                },
             cancellationToken);
        }

        private static CauseResponse MapToResponse(Cause cause)
        {
            return new CauseResponse()
            {
                Id = cause.Id,
                Name = cause.Name,
                Description = cause.Description
            };
        }
    }
}
