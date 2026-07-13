namespace Prisma.Domain.Entities;

public class PostMedia
{
    public Guid PostId { get; set; }

    public Guid MediaId { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime AddedAt { get; set; }


    public Post Post { get; set; } = null!;

    public Media Media { get; set; } = null!;
}

