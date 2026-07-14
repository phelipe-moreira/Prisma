using System;
using System.Collections.Generic;
using System.Text;

namespace Prisma.Domain.Models
{
    public sealed class TokenResult
    {
        public string AccessToken { get; init; } = string.Empty;
        public DateTime AccessTokenExpiresAt { get; init; }
        public string RefreshToken { get; init; } = string.Empty;
        public DateTime RefreshTokenExpiresAt { get; init; }
    }
}
