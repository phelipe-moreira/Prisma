namespace Prisma.Api.Responses;

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}

public class ApiResponse
{
    public bool Success { get; init; }
}
