using Prisma.Api.Responses;
using Prisma.Application.Results;

namespace Prisma.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToApiResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(new ApiResponse<T>
            {
                Success = true,
                Data = result.Value
            });
        }

        return Results.Problem(result.Error!.ToProblemDetails());
    }
}
