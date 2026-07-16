using FluentAssertions;
using Prisma.Application.Results;
using Prisma.Domain.Models;

namespace Prisma.Tests.Application.Results;

public class ErrorTests
{
    [Test]
    public void Constructor_ShouldSetAllProperties()
    {
        var error = new Error("USER_NOT_FOUND", "User not found", ErrorType.NotFound);

        error.Code.Should().Be("USER_NOT_FOUND");
        error.Message.Should().Be("User not found");
        error.Type.Should().Be(ErrorType.NotFound);
    }

    [Test]
    public void RecordEquality_ShouldCompareAllMembers()
    {
        var error1 = new Error("VALIDATION", "Invalid input", ErrorType.Validation);
        var error2 = new Error("VALIDATION", "Invalid input", ErrorType.Validation);
        var error3 = new Error("VALIDATION", "Different message", ErrorType.Validation);

        error1.Should().Be(error2);
        error1.Should().NotBe(error3);
    }
}
