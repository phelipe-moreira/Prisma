namespace Prisma.Domain.Entities;

public class PostLike
{
    public Guid UserId { get; set; }

    public Guid PostId { get; set; }

    public DateTime LikedAt { get; set; }


    public Post Post { get; set; } = null!;

    public User User { get; set; } = null!;

    private PostLike()
    {

    }

    public static PostLike Create(Guid userId, Guid postId)
    {
        return new PostLike
        {
            UserId = userId,
            PostId = postId,
            LikedAt = DateTime.UtcNow
        };
    }
}
