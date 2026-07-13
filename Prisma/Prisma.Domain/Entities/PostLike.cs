namespace Prisma.Domain.Entities;

public class PostLike
{
    public Guid UserId { get; set; }

    public Guid PostId { get; set; }

    public DateTime LikedAt { get; set; }


    public Post Post { get; set; } = null!;

    public User User { get; set; } = null!;
}
