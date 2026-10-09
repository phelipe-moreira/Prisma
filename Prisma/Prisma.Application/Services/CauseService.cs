using Prisma.Application.DTOs.Cause;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Models;

namespace Prisma.Application.Services;

public class CauseService(IUnitOfWork unitOfWork) : ICauseService
{
    public async Task<Result<CauseResponse>> CreateAsync(CreateCauseRequest request, CancellationToken cancellationToken)
    {
        var causeResult = await VerifyIfCauseExist(request, cancellationToken);

        if (causeResult.IsFailure)
            return Result<CauseResponse>.Failure(causeResult.Errors);

        var cause = Cause.Create(request.Name, request.Description);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.CauseRepository.AddAsync(cause, cancellationToken);

                return Result.Success();
            },
        cancellationToken);

        if (transactionResult.IsFailure)
            return Result<CauseResponse>.Failure(transactionResult.Errors);

        return Result.Success(MapToResponse(cause));
    }

    private async Task<Result> VerifyIfCauseExist(CreateCauseRequest request, CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.CauseRepository.ExistsByNameAsync(request.Name, cancellationToken);

        return exists ? Result.Failure(CauseErrors.AlreadyExists) : Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var cause = await unitOfWork.CauseRepository.GetByIdAsync(id, cancellationToken);

        if (cause is null)
            return Result.Failure(CauseErrors.NotFound);

        return await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.CauseRepository.DeleteAsync(cause, cancellationToken);

                return Result.Success();
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

        cause.Update(request.Name, request.Description);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.CauseRepository.UpdateAsync(cause, cancellationToken);

                return Result.Success();
            },
         cancellationToken);

        if (transactionResult.IsFailure)
            return Result<CauseResponse>.Failure(transactionResult.Errors);

        return Result.Success(MapToResponse(cause));
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
