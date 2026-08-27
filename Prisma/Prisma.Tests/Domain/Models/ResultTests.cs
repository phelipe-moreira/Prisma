using FluentAssertions;
using Prisma.Domain.Models;

namespace Prisma.Tests.Domain.Models;

public class ResultTests
{
    [Test]
    public void Success_ShouldCreateSuccessfulResult()
    {
        var result = Result<string>.Success("payload");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("payload");
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void Failure_ShouldCreateFailedResult()
    {
        var error = new Error("NOT_FOUND", "Resource not found", ErrorType.NotFound);

        var result = Result<string>.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().ContainSingle().Which.Should().Be(error);
    }
}
