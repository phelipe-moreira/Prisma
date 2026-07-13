namespace Prisma.Domain.Entities;

public class PostLink
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }

    public required string LinkTitle { get; set; }

    public required string Url { get; set; }

    public DateTime CreatedAt { get; set; }


    public Post Post { get; set; } = null!;
}
