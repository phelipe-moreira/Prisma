using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Api.Responses;
using Prisma.Domain.Models;

namespace Prisma.Tests.Api.Extensions;

public class ResultExtensionsTests
{
    [Test]
    public void ToApiResult_WhenSuccess_ShouldReturnOkWithApiResponse()
    {
        var result = Result<string>.Success("hello");

        var actionResult = result.ToApiResult();

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponse<string>>().Subject;
        response.Success.Should().BeTrue();
        response.Data.Should().Be("hello");
    }

    [Test]
    public void ToApiResult_WhenFailure_ShouldReturnErrorResponse()
    {
        var error = new Error("TEST_ERROR", "Something went wrong", ErrorType.NotFound);
        var result = Result<string>.Failure(error);

        var actionResult = result.ToApiResult();

        var objectResult = actionResult.Should().BeOfType<NotFoundObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var response = objectResult.Value.Should().BeOfType<ErrorResponse>().Subject;
        response.Errors.Should().ContainSingle().Which.Should().Be(error);
    }
}
