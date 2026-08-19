using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Responses;
using Prisma.Domain.Models;

namespace Prisma.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToApiResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(new ApiResponse<T>
            {
                Success = true,
                Data = result.Value
            });
        }

        return result.Errors.ToErrorResponse();
    }

    public static IActionResult ToApiResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(new ApiResponse
            {
                Success = true
            });
        }

        return result.Errors.ToErrorResponse();
    }
}
