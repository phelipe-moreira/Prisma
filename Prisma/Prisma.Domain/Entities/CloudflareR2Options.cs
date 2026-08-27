namespace Prisma.Domain.Entities;

public class CloudflareR2Options
{
    public required string AccountId { get; set; }
    public required string AccessKeyId { get; set; }
    public required string SecretAccessKey { get; set; }
    public required string BucketName { get; set; }
    public string? PublicUrl { get; set; }
}
