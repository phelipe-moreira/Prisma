using Prisma.Domain.Enums;

namespace Prisma.Application.DTOs.Media;

public class CompleteMediaUploadRequest
{
    public MediaType Type { get; set; }

    public string? StorageKey { get; set; }

    public int DisplayOrder { get; set; }
}
