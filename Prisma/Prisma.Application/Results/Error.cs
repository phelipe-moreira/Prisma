namespace Prisma.Application.Results;

public record Error(
    string Code,
    string Message,
    ErrorType Type
);