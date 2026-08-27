using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.Media;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers;

[ApiController]
[Route("api/posts/{postId:guid}/media")]
public class MediaController(
    IMediaService mediaService) : ControllerBase
{
    [Authorize]
    [HttpPost("upload-url")]
    public async Task<IActionResult> GetUploadUrl(Guid postId, [FromBody] UploadMediaRequest request, CancellationToken cancellationToken)
    {
        var result = await mediaService.GetUploadUrlAsync(
            postId,
            request,
            cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpPost("{mediaId:guid}/complete")]
    public async Task<IActionResult> CompleteUpload(
        Guid postId,
        Guid mediaId,
        [FromBody] CompleteMediaUploadRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediaService.CompleteUploadAsync(
            postId,
            mediaId,
            request,
            cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpGet("{mediaId:guid}")]
    public async Task<IActionResult> Get(Guid mediaId, CancellationToken cancellationToken)
    {
        var result = await mediaService.GetAsync(
            mediaId,
            cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpDelete("{mediaId:guid}")]
    public async Task<IActionResult> Delete(Guid mediaId, CancellationToken cancellationToken)
    {
        var result = await mediaService.DeleteAsync(
            mediaId,
            cancellationToken);

        return result.ToApiResult();
    }
}
