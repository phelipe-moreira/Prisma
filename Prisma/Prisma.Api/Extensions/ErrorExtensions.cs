using Microsoft.AspNetCore.Mvc;
using Prisma.Application.Results;

namespace Prisma.Api.Extensions;

public static class ErrorExtensions
{
    public static ProblemDetails ToProblemDetails(this Error error)
    {
        return new ProblemDetails
        {
            Title = error.Code,
            Detail = error.Message,
            Status = GetStatusCode(error.Type)
        };
    }

    private static int GetStatusCode(ErrorType type)
    {
        return type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
