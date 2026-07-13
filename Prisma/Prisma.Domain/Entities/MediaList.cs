namespace Prisma.Domain.Entities;

public class MediaList
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool IsPublic { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }


    public User User { get; set; } = null!;

    public ICollection<MediaListItem> MediaListItems { get; set; } = [];
}
