namespace Prisma.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public Guid UserId { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public bool IsRevoked => RevokedAt != null;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public User User { get; set; } = null!;

        private RefreshToken()
        {
        }

        public static RefreshToken Create(
            Guid userId,
            string token,
            DateTime expiresAt)
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt
            };
        }

        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }
    }
}
