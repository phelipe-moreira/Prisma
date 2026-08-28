namespace Prisma.Application.DTOs.Media;

public class UploadMediaRequest
{
    public required string ContentType { get; set; } = null!;

    public required long FileSize { get; set; } = 0;
}
