using Prisma.Application.DTOs.NgoCause;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services
{
    public class NgoCauseService(IUnitOfWork unitOfWork) : INgoCauseService
    {
        public async Task<Result<NgoCauseResponse>> CreateAsync(CreateNgoCauseRequest request, CancellationToken cancellationToken)
        {
            var ngo = await unitOfWork.NgoRepository.GetByIdAsync(request.NgoId, cancellationToken);

            if (ngo is null)
                return Result<NgoCauseResponse>.Failure(NgoCauseErrors.NgoNotFound);

            var cause = await unitOfWork.CauseRepository.GetByIdAsync(request.CauseId, cancellationToken);

            if(cause is null)
                return Result<NgoCauseResponse>.Failure(NgoCauseErrors.CauseNotFound);

            var exists = await unitOfWork.NgoCauseRepository.ExistsAsync(request.NgoId, request.CauseId, cancellationToken);

            if(exists)
                return Result<NgoCauseResponse>.Failure(NgoCauseErrors.AlreadyExists);

            return await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    var ngoCause = NgoCause.Create(request.NgoId, request.CauseId);

                    await unitOfWork.NgoCauseRepository.AddAsync(ngoCause, cancellationToken);

                    return Result<NgoCauseResponse>.Success(MapToResponse(ngoCause, ngo.Name, cause.Name));
                },
                cancellationToken);
        }

        public async Task DeleteAsync(Guid ngoId, Guid causeId, CancellationToken cancellationToken)
        {
            var ngoCause = await unitOfWork.NgoCauseRepository.GetAsync(ngoId, causeId, cancellationToken);

            if (ngoCause is null)
                return;

            await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.NgoCauseRepository.DeleteAsync(ngoCause, cancellationToken);

                    return Result<bool>.Success(true);

                }, cancellationToken);
        }

        public async Task<Result<IEnumerable<NgoCauseResponse>>> GetByCauseIdAsync(Guid causeId, CancellationToken cancellationToken)
        {
            var cause = await unitOfWork.CauseRepository.GetByIdAsync(causeId, cancellationToken);

            if (cause is null)
                return Result<IEnumerable<NgoCauseResponse>>.Failure(NgoCauseErrors.CauseNotFound);

            var ngoCauses = await unitOfWork.NgoCauseRepository.GetByCauseIdAsync(causeId, cancellationToken);

            var response = ngoCauses.Select(ngoCause => MapToResponse(ngoCause, ngoCause.Ngo.Name, cause.Name));

            return Result<IEnumerable<NgoCauseResponse>>.Success(response);
        }

        public async Task<Result<IEnumerable<NgoCauseResponse>>> GetByNgoIdAsync(Guid ngoId, CancellationToken cancellationToken)
        {
            var ngo = await unitOfWork.NgoRepository.GetByIdAsync(ngoId, cancellationToken);

            if(ngo is null)
                return Result<IEnumerable<NgoCauseResponse>>.Failure(NgoCauseErrors.NgoNotFound);

            var ngoCauses = await unitOfWork.NgoCauseRepository.GetByNgoIdAsync(ngoId, cancellationToken);

            var response = ngoCauses.Select(ngoCause => MapToResponse(ngoCause, ngo.Name, ngoCause.Cause.Name));

            return Result<IEnumerable<NgoCauseResponse>>.Success(response);
        }

        private static NgoCauseResponse MapToResponse(NgoCause ngoCause, string ngoName, string causeName)
        {
            return new NgoCauseResponse
            {
                NgoId = ngoCause.NgoId,
                NgoName = ngoName,
                CauseId = ngoCause.CauseId,
                CauseName = causeName
            };
        }
    }
}
