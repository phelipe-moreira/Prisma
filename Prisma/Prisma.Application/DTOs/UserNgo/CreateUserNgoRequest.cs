using Prisma.Domain.Enums;

namespace Prisma.Application.DTOs.UserNgo
{
    public class CreateUserNgoRequest
    {
        public Guid UserId { get; set; }
        public Guid NgoId { get; set; }
        public UserNgoRole Role { get; set; }
    }
}
