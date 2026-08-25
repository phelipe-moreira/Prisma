namespace Prisma.Domain.Entities;

public class SavedPost
{
    public Guid UserId { get; set; }

    public Guid PostId { get; set; }

    public DateTime SavedAt { get; set; }


    public User User { get; set; } = null!;

    public Post Post { get; set; } = null!;

    private SavedPost()
    {

    }

    public static SavedPost Create(Guid userId, Guid postId)
    {
        return new SavedPost()
        {
            UserId = userId,
            PostId = postId,
            SavedAt = DateTime.UtcNow
        };
    }
}
