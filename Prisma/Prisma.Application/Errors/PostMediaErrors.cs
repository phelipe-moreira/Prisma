using Prisma.Domain.Models;

namespace Prisma.Application.Errors;

public static class PostMediaErrors
{
    public static readonly Error NotFound = new("PostMedia.NotFound", "A mídia não está associada ao post.", ErrorType.NotFound);

    public static readonly Error AlreadyExists = new("PostMedia.AlreadyExists", "A mídia já está associada ao post.", ErrorType.Conflict);
}
