using Prisma.Domain.Models;

namespace Prisma.Application.Errors;

public static class UserErrors
{
    public static readonly Error NotFound = new("User.NotFound", "Usuário não encontrado.", ErrorType.NotFound);
    public static readonly Error Inactive = new("User.Inactive", "Usuário desativado", ErrorType.Forbidden);
    public static readonly Error InvalidCurrentPassword = new("User.InvalidCurrentPassword", "A senha atual está incorreta", ErrorType.Unauthorized);
}
