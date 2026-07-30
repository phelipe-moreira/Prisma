using Prisma.Domain.Enums;

namespace Prisma.Application.DTOs.UserNgo
{
    public class UserNgoResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid NgoId { get; set; }
        public string NgoName { get; set; } = string.Empty;
        public UserNgoRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
