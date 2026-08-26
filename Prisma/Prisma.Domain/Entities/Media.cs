using Prisma.Domain.Enums;

namespace Prisma.Domain.Entities;

public class Media
{
    public Guid Id { get; set; }

    public MediaType Type { get; set; }

    public required string Url { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<PostMedia> PostMedias { get; set; } = [];

    private Media() { Id = Guid.NewGuid(); }

    public static Media Create(MediaType type, string url, string? title, string? description)
        => new()
        {
            Type = type,
            Url = url,
            Title = title,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
}
