using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Prisma.Application.Interfaces;
using Prisma.Domain.Entities;
using System.Net;

namespace Prisma.Infrastructure.Storage;

public class CloudflareR2StorageService(IAmazonS3 s3Client, IOptions<CloudflareR2Options> options) : IStorageService
{
    public async Task DeleteAsync(string storageKey, CancellationToken cancellationToken)
    {
        await s3Client.DeleteObjectAsync(
            options.Value.BucketName,
            storageKey,
            cancellationToken);
    }

    public async Task<bool> ExistsAsync(string storageKey, CancellationToken cancellationToken)
    {
        try
        {
            await s3Client.GetObjectMetadataAsync(
                options.Value?.BucketName,
                storageKey,
                cancellationToken);

            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public string GenerateUploadUrl(string storageKey, string contentType, TimeSpan expiration)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = options.Value?.BucketName,
            Key = storageKey,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.Add(expiration),
            ContentType = contentType
        };

        return s3Client.GetPreSignedURL(request);
    }

    public string GetPublicUrl(string storageKey) 
        => $"{options.Value?.PublicUrl?.TrimEnd('/')}/{storageKey}";
}
