using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Api.Responses;
using Prisma.Application.Results;
using Prisma.Domain.Models;

namespace Prisma.Tests.Api.Extensions;

public class ResultExtensionsTests
{
    private sealed class TestController : ControllerBase;

    [Test]
    public void ToApiResult_WhenSuccess_ShouldReturnOkWithApiResponse()
    {
        var controller = CreateController();
        var result = Result<string>.Success("hello");

        var actionResult = result.ToApiResult(controller);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponse<string>>().Subject;
        response.Success.Should().BeTrue();
        response.Data.Should().Be("hello");
    }

    [Test]
    public void ToApiResult_WhenFailure_ShouldReturnProblemWithErrorDetails()
    {
        var controller = CreateController();
        var error = new Error("TEST_ERROR", "Something went wrong", ErrorType.NotFound);
        var result = Result<string>.Failure(error);

        var actionResult = result.ToApiResult(controller);

        var objectResult = actionResult.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var problemDetails = objectResult.Value.Should().BeOfType<ProblemDetails>().Subject;
        problemDetails.Title.Should().Be("TEST_ERROR");
        problemDetails.Detail.Should().Be("Something went wrong");
        problemDetails.Status.Should().Be(StatusCodes.Status404NotFound);
    }

    private static TestController CreateController() =>
        new()
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
}
