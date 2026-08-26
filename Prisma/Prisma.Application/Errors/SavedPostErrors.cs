using Prisma.Domain.Models;

namespace Prisma.Application.Errors
{
    public static class SavedPostErrors
    {
        public static readonly Error AlreadySaved = new("SavedPost.AlreadySaved", "O posta já está salvo", ErrorType.Conflict);
        public static readonly Error NotFound = new("SavedPost.NotFound", "O post salvo não foi encontrado", ErrorType.NotFound);
        public static readonly Error PostNotFound = new("SavedPost.PostNotFound", "O post não encontrado", ErrorType.NotFound);
    }
}
