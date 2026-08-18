using Prisma.Domain.Models;

namespace Prisma.Application.Errors
{
    public static class UserNgoErrors
    {
        public static readonly Error AlreadyExists = new("UserNgo.AlreadyExists", "User is already associated with this NGO.", ErrorType.Conflict);

        public static readonly Error UserNotFound = new("UserNgo.UserNotFound", "User was not found.", ErrorType.NotFound);

        public static readonly Error NgoNotFound = new("UserNgo.NgoNotFound", "NGO was not found.", ErrorType.NotFound);

        public static readonly Error NotFound = new("UserNgo.NotFound", "User NGO relationship was not found.", ErrorType.NotFound);
    }
}
