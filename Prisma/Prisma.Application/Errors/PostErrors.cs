using Prisma.Domain.Models;

namespace Prisma.Application.Errors;

public static class PostErrors
{
    public static readonly Error RequestCannotBeNull = new("Post.RequestCannotBeNull", "A requisição do post não pode ser nula.", ErrorType.Validation);
    public static readonly Error NotFound = new("Post.NotFound", "Post não encontrado.", ErrorType.NotFound);
    public static readonly Error Forbidden = new("Post.Forbidden", "Você não tem permissão para gerenciar este post.", ErrorType.Forbidden);
}
