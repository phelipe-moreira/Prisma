using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Responses;
using Prisma.Domain.Models;

namespace Prisma.Api.Extensions;

public static class ErrorExtensions
{
    public static IActionResult ToErrorResponse(
        this IReadOnlyCollection<Error> errors)
    {
        var errorType = errors
            .Select(x => x.Type)
            .GetPriorityType();

        var response = new ErrorResponse(
            [.. errors.Where(x => x.Type == errorType)]);

        return errorType switch
        {
            ErrorType.Validation => new BadRequestObjectResult(response),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(response),
            ErrorType.Forbidden => new ForbidResult(),
            ErrorType.NotFound => new NotFoundObjectResult(response),
            ErrorType.Conflict => new ConflictObjectResult(response),
            _ => new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }

    public static ErrorType GetPriorityType(
    this IEnumerable<ErrorType> types)
    {
        if (types.Contains(ErrorType.Validation))
            return ErrorType.Validation;

        if (types.Contains(ErrorType.Unauthorized))
            return ErrorType.Unauthorized;

        if (types.Contains(ErrorType.Forbidden))
            return ErrorType.Forbidden;

        if (types.Contains(ErrorType.NotFound))
            return ErrorType.NotFound;

        return ErrorType.Failure;
    }
}
