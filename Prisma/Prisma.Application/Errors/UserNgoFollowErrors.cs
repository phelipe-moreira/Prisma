using Prisma.Domain.Models;

namespace Prisma.Application.Errors
{
    public static class UserNgoFollowErrors
    {
        public static readonly Error UserNotFound = new("UserNgoFollow.UserNotFound", "Usuário não encontrado.", ErrorType.NotFound);

        public static readonly Error NgoNotFound = new("UserNgoFollow.NgoNotFound", "ONG não encontrada.", ErrorType.NotFound);

        public static readonly Error AlreadyExists = new("UserNgoFollow.AlreadyExists", "O usuário já segue esta ONG.", ErrorType.Conflict);

        public static readonly Error NotFound = new("UserNgoFollow.NotFound", "O relacionamento entre usuário e ONG não foi encontrado.", ErrorType.NotFound);
    }
}
