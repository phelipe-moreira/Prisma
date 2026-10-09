using Prisma.Domain.Models;

namespace Prisma.Application.Errors
{
    public static class CauseErrors
    {
        public static readonly Error AlreadyExists = new("Cause.AlreadyExists", "Já existe uma causa cadastrada com esse nome.", ErrorType.Conflict);

        public static readonly Error NotFound = new("Cause.NotFound", "Causa não encontrada.", ErrorType.NotFound);
    }
}
