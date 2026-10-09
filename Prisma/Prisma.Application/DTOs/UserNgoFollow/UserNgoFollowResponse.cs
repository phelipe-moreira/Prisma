namespace Prisma.Application.DTOs.UserNgoFollow
{
    public class UserNgoFollowResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid NgoId { get; set; }
        public string NgoName { get; set; } = string.Empty;
        public DateTime FollowedAt { get; set; }
    }
}
