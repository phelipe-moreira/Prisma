using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Prisma.Api.Extensions;
using Prisma.Application.Results;
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
    public void ToProblemDetails_ShouldMapErrorTypeToStatusCode(ErrorType errorType, int expectedStatusCode)
    {
        var error = new Error("ERROR_CODE", "Error message", errorType);

        var problemDetails = error.ToProblemDetails();

        problemDetails.Title.Should().Be("ERROR_CODE");
        problemDetails.Detail.Should().Be("Error message");
        problemDetails.Status.Should().Be(expectedStatusCode);
    }
}
