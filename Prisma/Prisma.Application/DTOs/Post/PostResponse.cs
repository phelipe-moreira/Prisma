using Prisma.Domain.Entities;
using Prisma.Domain.Enums;

namespace Prisma.Application.DTOs.Post;

public class PostResponse
{
    public Guid Id { get; set; }

    public Guid NgoId { get; set; }

    public required string Title { get; set; }

    public string? Content { get; set; }

    public PostStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<Domain.Entities.Comment> Comments { get; set; } = [];

    public int Likes { get; set; }
}
