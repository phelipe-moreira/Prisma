using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Responses;
using Prisma.Domain.Models;

namespace Prisma.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToApiResult<T>(this Result<T> result, ControllerBase controller)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(new ApiResponse<T>
            {
                Success = true,
                Data = result.Value
            });
        }

        var problem = result.Error!.ToProblemDetails();

        return controller.Problem(
            detail: problem.Detail,
            title: problem.Title,
            statusCode: problem.Status);
    }
}
