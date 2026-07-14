using FluentAssertions;
using Prisma.Application.Results;

namespace Prisma.Tests.Application.Results;

public class ResultTests
{
    [Test]
    public void Success_ShouldCreateSuccessfulResult()
    {
        var result = Result<string>.Success("payload");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("payload");
        result.Error.Should().BeNull();
    }

    [Test]
    public void Failure_ShouldCreateFailedResult()
    {
        var error = new Error("NOT_FOUND", "Resource not found", ErrorType.NotFound);

        var result = Result<string>.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().Be(error);
    }
}
