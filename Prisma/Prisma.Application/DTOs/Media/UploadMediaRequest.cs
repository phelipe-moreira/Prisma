namespace Prisma.Application.DTOs.Media;

public class UploadMediaRequest
{
    public required string FileName { get; set; }

    public required string ContentType { get; set; }

    public required long FileSize { get; set; }
}
