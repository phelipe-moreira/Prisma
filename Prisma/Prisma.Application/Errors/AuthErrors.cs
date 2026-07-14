using Prisma.Application.Results;

namespace Prisma.Application.Errors
{
    public static class AuthErrors
    {
        public static readonly Error InvalidCredentials = new("AUTH.INVALID_CREDENTIALS", "E-mail ou senha inválidos.", ErrorType.Unauthorized);

        public static readonly Error UserInactive = new("AUTH.USER_INACTIVE", "Usuário desativado.", ErrorType.Forbidden);

        public static readonly Error EmailAlreadyExists = new("AUTH.EMAIL_ALREADY_EXISTS", "Já existe um usuário com este e-mail.", ErrorType.Conflict);

        public static readonly Error InvalidRefreshToken = new("AUTH.INVALID_REFRESH_TOKEN", "Refresh Token inválido.", ErrorType.Unauthorized);

        public static readonly Error RefreshTokenExpired = new("AUTH.REFRESH_TOKEN_EXPIRED", "Refresh Token expirado.", ErrorType.Unauthorized);

        public static readonly Error RefreshTokenRevoked = new("AUTH.REFRESH_TOKEN_REVOKED", "Refresh Token revogado.", ErrorType.Unauthorized);

        public static readonly Error UserNotFound = new("AUTH.USER_NOT_FOUND", "Usuário não encontrado.", ErrorType.NotFound);
    }
}
