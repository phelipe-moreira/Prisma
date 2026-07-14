using Prisma.Domain.Entities;
using Prisma.Domain.Models;
using System.Security.Claims;

namespace Prisma.Domain.Interfaces.Security
{
    public interface IJwtTokenService
    {
        TokenResult GenerateTokens(User user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
