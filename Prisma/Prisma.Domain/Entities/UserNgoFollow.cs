namespace Prisma.Domain.Entities;

public class UserNgoFollow
{
    public Guid UserId { get; set; }

    public Guid NgoId { get; set; }

    public DateTime FollowedAt { get; set; }


    public User User { get; set; } = null!;

    public Ngo Ngo { get; set; } = null!;

    private UserNgoFollow()
    {
    }

    public static UserNgoFollow Create(Guid userId, Guid ngoId)
    {
        return new UserNgoFollow
        {
            UserId = userId,
            NgoId = ngoId,
            FollowedAt = DateTime.UtcNow
        };
    }
}
