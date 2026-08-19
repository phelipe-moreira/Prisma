using Prisma.Domain.Models;

namespace Prisma.Api.Responses;

public class ErrorResponse
{    
    public ErrorResponse() { }

    public ErrorResponse(List<Error> errors) 
    {
        Errors.AddRange(errors);
    }

    public List<Error> Errors { get; set; } = [];
}
