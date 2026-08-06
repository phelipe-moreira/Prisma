using Prisma.Domain.Models;

namespace Prisma.Application.Errors
{
    public static class NgoErrors
    {
        public static readonly Error AlreadyExists = new("Ngo.AlreadyExists", "Já existe uma ONG cadastrada com este CNPJ.", ErrorType.Conflict);

        public static readonly Error NotFound = new("Ngo.NotFound", "ONG não encontrada.", ErrorType.NotFound);

        public static readonly Error CauseNotFound = new("Ngo.CauseNotFound", "Uma das causas informadas não foi encontrada.", ErrorType.NotFound);
    }
}
