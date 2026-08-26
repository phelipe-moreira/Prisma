using Prisma.Domain.Models;

namespace Prisma.Application.Errors;

public static class MediaErrors
{    
    public static readonly Error NotFound = new("Media.NotFound", "Media não encontrada.", ErrorType.NotFound);
}