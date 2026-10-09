using FluentAssertions;
using Prisma.Api.Responses;

namespace Prisma.Tests.Api.Responses;

public class ApiResponseTests
{
    [Test]
    public void Properties_ShouldBeSettable()
    {
        var response = new ApiResponse<string>
        {
            Success = true,
            Data = "test-data"
        };

        response.Success.Should().BeTrue();
        response.Data.Should().Be("test-data");
    }
}
