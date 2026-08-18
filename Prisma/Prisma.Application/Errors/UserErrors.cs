using Prisma.Domain.Models;

namespace Prisma.Application.Errors;

public static class UserErrors
{
    public static readonly Error NotFound = new("User.NotFound", "Usuário não encontrado.", ErrorType.NotFound);

    public static readonly Error SomeNotFound = new("User.SomeNotFound", "Um ou mais usuários não foram encontrados.", ErrorType.NotFound);
}
