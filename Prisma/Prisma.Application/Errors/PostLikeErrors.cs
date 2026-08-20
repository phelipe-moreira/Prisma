using Prisma.Domain.Models;

namespace Prisma.Application.Errors
{
    public static class PostLikeErrors
    {
        public static readonly Error PostNotFound = new("PostLike.PostNotFound", "Não foi encontrado o post", ErrorType.NotFound);
        public static readonly Error AlreadyLiked = new("PostLike.AlreadyLiked", "O usuário já deu like nesse post", ErrorType.Conflict);
        public static readonly Error NotLiked = new("PostLike.NotLiked", "O usuário não deu like nesse post", ErrorType.NotFound);
    }
}
