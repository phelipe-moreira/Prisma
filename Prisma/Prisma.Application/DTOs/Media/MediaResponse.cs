using Prisma.Domain.Enums;

namespace Prisma.Application.DTOs.Media;

public class MediaResponse
{
    public Guid Id { get; set; }

    public MediaType Type { get; set; }

    public string? StorageKey { get; set; }

    public string? Url { get; set; }

    public DateTime CreatedAt { get; set; }
}
