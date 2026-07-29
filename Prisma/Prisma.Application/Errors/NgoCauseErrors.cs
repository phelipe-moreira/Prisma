using Prisma.Domain.Models;

namespace Prisma.Application.Errors
{
    public static class NgoCauseErrors
    {
        public static readonly Error AlreadyExists = new("NgoCause.AlreadyExists", "Esta ONG já está associada a esta causa.", ErrorType.Conflict);

        public static readonly Error NotFound = new("NgoCause.NotFound", "Associação entre ONG e causa não encontrada.", ErrorType.NotFound);

        public static readonly Error NgoNotFound = new("NgoCause.NgoNotFound", "ONG não encontrada.", ErrorType.NotFound);

        public static readonly Error CauseNotFound = new("NgoCause.CauseNotFound", "Causa não encontrada.", ErrorType.NotFound);
    }
}
