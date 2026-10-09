namespace Prisma.Domain.Models;

public record Error(
    string Code,
    string Message,
    ErrorType Type
)
{
    public object? Value { get; set; } = null;
};