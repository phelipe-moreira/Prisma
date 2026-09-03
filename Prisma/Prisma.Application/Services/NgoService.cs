using Prisma.Application.DTOs.Cause;
using Prisma.Application.DTOs.Ngo;
using Prisma.Application.Errors;
using Prisma.Application.Interfaces;
using Prisma.Domain.Abstractions;
using Prisma.Domain.Entities;
using Prisma.Domain.Enums;
using Prisma.Domain.Models;
using System.Xml.Linq;

namespace Prisma.Application.Services;

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

        var member = await unitOfWork.UserRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (member is null)
            return Result<NgoResponse>.Failure(UserErrors.NotFound);

        ngo.AddMember(request.UserId, UserNgoRole.Admin);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.NgoRepository.AddAsync(ngo, cancellationToken);

                return Result.Success();
            },
            cancellationToken);

        if (transactionResult.IsFailure)
            return Result<NgoResponse>.Failure(transactionResult.Errors);

        return Result.Success(MapToResponse(ngo));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var ngo = await unitOfWork.NgoRepository.GetByIdAsync(id, cancellationToken);

        if (ngo is null)
            return Result.Failure(NgoErrors.NotFound);

        ngo.Deactivate();

        return await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.NgoRepository.DeleteAsync(ngo, cancellationToken);

                return Result.Success();
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

        var verifyCauseResult = await VerifyIfCausesExists(request, cancellationToken);

        if (verifyCauseResult.IsFailure)
            return Result<NgoResponse>.Failure(verifyCauseResult.Errors);

        ngo.UpdateCauses(request.CauseIds);

        var verifyMemberResult = await VerifyIfMembersExists(request, cancellationToken);

        if (verifyMemberResult.IsFailure)
            return Result<NgoResponse>.Failure(verifyMemberResult.Errors);

        ngo.UpdateMembers(request.Members);

        var transactionResult = await unitOfWork.ExecuteTransactionAsync(
            async () =>
            {
                await unitOfWork.NgoRepository.UpdateAsync(ngo, cancellationToken);

                return Result<NgoResponse>.Success(MapToResponse(ngo));

            }, cancellationToken);

        if (transactionResult.IsFailure)
            return Result<NgoResponse>.Failure(transactionResult.Errors);

        return Result.Success(MapToResponse(ngo));
    }

    private async Task<Result> VerifyIfCausesExists(UpdateNgoRequest request, CancellationToken cancellationToken)
    {
        var causes = await unitOfWork.CauseRepository
            .GetAllAsync(cancellationToken);

        var causesIds = causes
            .Select(x => x.Id)
            .ToList();

        var missingIds = request.CauseIds
            .Except(causesIds)
            .ToList();

        if (missingIds is { Count: > 0 })
        {
            var errors = missingIds.Select(missingId =>
            {
                var error = NgoErrors.CauseNotFound;
                error.Value = missingId;

                return error;
            })
            .ToList();

            return Result.Failure(errors);
        }

        return Result.Success();
    }

    private async Task<Result> VerifyIfMembersExists(UpdateNgoRequest request, CancellationToken cancellationToken)
    {
        var members = await unitOfWork.UserRepository
            .GetAllAsync(cancellationToken);

        var memberIds = members
            .Select(x => x.Id)
            .ToList();

        var missingIds = request.Members.Keys
            .Except(memberIds)
            .ToList();

        if (missingIds is { Count: > 0 })
        {
            var errors = missingIds.Select(missingId =>
            {
                var error = UserErrors.NotFound;
                error.Value = missingId;

                return error;
            })
            .ToList();

            return Result.Failure(errors);
        }

        return Result.Success();
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
            Causes = [.. ngo.NgoCauses.Select(x => new CauseResponse
            {
                Id = x.Cause.Id,
                Name = x.Cause.Name,
                Description = x.Cause.Description
            })],
            Members = [.. ngo.Members.Select(x => new UserNgoResponse
            {                
                UserId = x.UserId,
                Name = x.User?.Name,
                Role = x.Role
            })]
        };
    }
}
