using Prisma.Domain.Enums;

namespace Prisma.Domain.Entities;

public class Media
{
    public Guid Id { get; set; }

    public MediaType Type { get; set; }

    public required string StorageKey { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<PostMedia> PostMedias { get; set; } = [];

    private Media() { }

    public static Media Create(Guid Id, MediaType type, string storageKey)
        => new()
        {
            Id = Id,
            Type = type,
            StorageKey = storageKey,
            CreatedAt = DateTime.UtcNow
        };
}
