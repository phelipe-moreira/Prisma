namespace Prisma.Application.DTOs.Media;

public class UploadMediaResponse
{
    public Guid Id { get; set; }

    public string? StorageKey { get; set; }

    public string? UploadUrl { get; set; }
}
