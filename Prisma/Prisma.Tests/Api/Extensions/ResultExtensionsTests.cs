using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Prisma.Api.Extensions;
using Prisma.Api.Responses;
using Prisma.Application.Results;

namespace Prisma.Tests.Api.Extensions;

public class ResultExtensionsTests
{
    [Test]
    public void ToApiResult_WhenSuccess_ShouldReturnOkWithApiResponse()
    {
        var result = Result<string>.Success("hello");

        var apiResult = result.ToApiResult();

        var okResult = apiResult.Should().BeOfType<Ok<ApiResponse<string>>>().Subject;
        okResult.Value!.Success.Should().BeTrue();
        okResult.Value.Data.Should().Be("hello");
    }

    [Test]
    public void ToApiResult_WhenFailure_ShouldReturnProblem()
    {
        var error = new Error("TEST_ERROR", "Something went wrong", ErrorType.NotFound);
        var result = Result<string>.Failure(error);

        var apiResult = result.ToApiResult();

        apiResult.Should().BeOfType<ProblemHttpResult>();
    }
}
