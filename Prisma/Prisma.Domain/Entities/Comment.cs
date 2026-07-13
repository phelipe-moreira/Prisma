namespace Prisma.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }

    public Guid UserId { get; set; }

    public Guid ParentCommentId { get; set; }

    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsRemoved { get; set; }


    public Post Post { get; set; } = null!;

    public User User { get; set; } = null!;

    public Comment? ParentComment { get; set; }

    public ICollection<Comment> Replies { get; set; } = [];
}
