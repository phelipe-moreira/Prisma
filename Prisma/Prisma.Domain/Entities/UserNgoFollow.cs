namespace Prisma.Domain.Entities;

public class UserNgoFollow
{
    public Guid UserId { get; set; }

    public Guid NgoId { get; set; }

    public DateTime FollowedAt { get; set; }


    public User User { get; set; } = null!;

    public Post Post { get; set; } = null!;
}
