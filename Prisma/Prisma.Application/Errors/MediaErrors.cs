using Prisma.Domain.Models;

namespace Prisma.Application.Errors;

public static class MediaErrors
{
    public static readonly Error RequestCannotBeNull = new("Media.NotFound", "A requisição da mídia não pode ser nula.", ErrorType.Validation);
    public static readonly Error NotFound = new("Media.NotFound", "Mídia não encontrada.", ErrorType.NotFound);
    public static readonly Error InvalidContentType = new("Media.InvalidContentType", "Tipo do arquivo inválido", ErrorType.Validation);
    public static readonly Error FileTooLarge = new("Media.FileTooLarge", "O arquivo excede o tamanho máximo permitido.", ErrorType.Validation);
    public static readonly Error UploadNotFound = new("Media.UploadNotFound", "Arquivo não encontrado", ErrorType.NotFound);
}