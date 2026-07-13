using Prisma.Domain.Enums;

namespace Prisma.Domain.Entities;

public class Media
{
    public Guid Id { get; set; }

    public MediaSource Source { get; set; }

    public string? ExternalId { get; set; }

    public MediaType Type { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? LinkUrl { get; set; }

    public DateTime ReleaseDate { get; set; }

    public DateTime CreatedAt { get; set; }


    public ICollection<PostMedia> PostMedias { get; set; } = [];
}
