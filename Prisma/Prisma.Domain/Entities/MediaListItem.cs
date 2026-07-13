namespace Prisma.Domain.Entities;

public class MediaListItem
{
    public Guid MediaListId { get; set; }

    public Guid MediaId { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime AddedAt { get; set; }


    public Media Media { get; set; } = null!;

    public MediaList MediaList { get; set; } = null!;
}
