using Prisma.Application.DTOs.Cause;
using Prisma.Application.DTOs.Ngo;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services
{
    public class NgoService(IUnitOfWork unitOfWork) : INgoService
    {
        public async Task<Result<NgoResponse>> CreateAsync(CreateNgoRequest request, CancellationToken cancellationToken = default)
        {
            var exists = await unitOfWork.NgoRepository.ExistsByCnpjAsync(request.Cnpj, cancellationToken);

            if (exists)
                return Result<NgoResponse>.Failure(NgoErrors.AlreadyExists);

            var ngo = Ngo.Create(
                request.Name,
                request.Cnpj,
                request.Description,
                request.ProfilePictureUrl,
                request.CoverPictureUrl,
                request.WebsiteUrl,
                request.InstagramUrl,
                request.ContactEmail,
                request.City,
                request.State
            );

            foreach (var causeId in request.CauseIds)
            {
                var cause = await unitOfWork.CauseRepository.GetByIdAsync(causeId, cancellationToken);

                if (cause is null)
                    return Result<NgoResponse>.Failure(NgoErrors.CauseNotFound);

                ngo.AddCause(causeId);
            }

            return await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.NgoRepository.AddAsync(ngo, cancellationToken);

                    return Result<NgoResponse>.Success(MapToResponse(ngo));
                },
                cancellationToken);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var ngo = await unitOfWork.NgoRepository.GetByIdAsync(id, cancellationToken);

            if (ngo is null)
                return Result<bool>.Failure(NgoErrors.NotFound);

            ngo.Deactivate();

            return await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.NgoRepository.UpdateAsync(ngo, cancellationToken);

                    return Result<bool>.Success(true);
                },
                cancellationToken);
        }

        public async Task<Result<IEnumerable<NgoResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var ngos = await unitOfWork.NgoRepository.GetAllAsync(cancellationToken);

            var response = ngos.Select(MapToResponse);

            return Result<IEnumerable<NgoResponse>>.Success(response);
        }

        public async Task<Result<NgoResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var ngo = await unitOfWork.NgoRepository.GetByIdAsync(id, cancellationToken);

            if (ngo is null)
                return Result<NgoResponse>.Failure(NgoErrors.NotFound);

            return Result<NgoResponse>.Success(MapToResponse(ngo));
        }

        public async Task<Result<NgoResponse>> UpdateAsync(Guid id, UpdateNgoRequest request, CancellationToken cancellationToken = default)
        {
            var ngo = await unitOfWork.NgoRepository.GetByIdAsync(id, cancellationToken);

            if (ngo is null)
                return Result<NgoResponse>.Failure(NgoErrors.NotFound);

            ngo.Update(
                request.Name,
                request.Description,
                request.ProfilePictureUrl,
                request.CoverPictureUrl,
                request.WebsiteUrl,
                request.InstagramUrl,
                request.ContactEmail,
                request.City,
                request.State
            );

            ngo.UpdateCauses(request.CauseIds);

            return await unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await unitOfWork.NgoRepository.UpdateAsync(ngo, cancellationToken);

                    return Result<NgoResponse>.Success(MapToResponse(ngo));

                }, cancellationToken);
        }

        private static NgoResponse MapToResponse(Ngo ngo)
        {
            return new NgoResponse
            {
                Id = ngo.Id,
                Name = ngo.Name,
                Cnpj = ngo.Cnpj,
                Description = ngo.Description,
                ProfilePictureUrl = ngo.ProfilePictureUrl,
                CoverPictureUrl = ngo.CoverPictureUrl,
                WebsiteUrl = ngo.WebsiteUrl,
                InstagramUrl = ngo.InstagramUrl,
                ContactEmail = ngo.ContactEmail,
                City = ngo.City,
                State = ngo.State,
                IsActive = ngo.IsActive,
                CreatedAt = ngo.CreatedAt,
                UpdatedAt = ngo.UpdatedAt,

                Causes = ngo.NgoCauses.Select(x => new CauseResponse
                {
                    Id = x.Cause.Id,
                    Name = x.Cause.Name,
                    Description = x.Cause.Description
                }).ToList()
            };
        }
    }
}
