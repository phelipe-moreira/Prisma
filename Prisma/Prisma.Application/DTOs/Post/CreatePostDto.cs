using Prisma.Domain.Enums;

namespace Prisma.Application.DTOs.Post;

public class CreatePostDto
{
    public Guid NgoId { get; set; }

    public required string Title { get; set; }

    public string? Content { get; set; }

    public PostStatus Status { get; set; }
}
