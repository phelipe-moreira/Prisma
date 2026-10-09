using Prisma.Domain.Models;

namespace Prisma.Application.Errors;

public static class CommentErros
{
    public static readonly Error RequestCannotBeNull = new("Comment.RequestCannotBeNull", "A requisição do comentário não pode ser nula.", ErrorType.Validation);
    public static readonly Error NotFound = new("Comment.NotFound", "Comentário não encontrado.", ErrorType.NotFound);
    public static readonly Error Unauthorized = new("Comment.Unauthorized", "Sem autorização", ErrorType.Unauthorized);
}
