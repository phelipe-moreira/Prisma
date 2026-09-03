using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Responses;
using Prisma.Api.Extensions;
using Prisma.Domain.Models;

namespace Prisma.Tests.Api.Extensions;

public class ErrorExtensionsTests
{
    [TestCase(ErrorType.Validation, StatusCodes.Status400BadRequest)]
    [TestCase(ErrorType.Unauthorized, StatusCodes.Status401Unauthorized)]
    [TestCase(ErrorType.Forbidden, StatusCodes.Status403Forbidden)]
    [TestCase(ErrorType.NotFound, StatusCodes.Status404NotFound)]
    [TestCase(ErrorType.Conflict, StatusCodes.Status409Conflict)]
    [TestCase(ErrorType.Failure, StatusCodes.Status500InternalServerError)]
    public void ToErrorResponse_ShouldMapErrorTypeToHttpResponse(ErrorType errorType, int expectedStatusCode)
    {
        var error = new Error("ERROR_CODE", "Error message", errorType);

        var actionResult = new[] { error }.ToErrorResponse();

        var objectResult = actionResult.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(expectedStatusCode);

        var response = objectResult.Value.Should().BeOfType<ErrorResponse>().Subject;
        response.Errors.Should().ContainSingle().Which.Should().Be(error);
    }
}
