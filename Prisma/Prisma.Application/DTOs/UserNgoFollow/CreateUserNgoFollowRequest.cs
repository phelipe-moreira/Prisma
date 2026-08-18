namespace Prisma.Application.DTOs.UserNgoFollow
{
    public class CreateUserNgoFollowRequest
    {
        public Guid UserId { get; set; }
        public Guid NgoId { get; set; }
    }
}
