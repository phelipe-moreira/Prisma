namespace Prisma.Application.DTOs.Auth
{
    public sealed class AuthToken
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; init; }
    }
}
