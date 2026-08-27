using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.Media;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers;

[ApiController]
[Route("api/post")]
public class MediaController(
    IMediaService mediaService) : ControllerBase
{
    [Authorize]
    [HttpPost("{postId:guid}/media/upload-url")]
    public async Task<IActionResult> GetUploadUrl(Guid postId, [FromBody] UploadMediaRequest request, CancellationToken cancellationToken)
    {
        var result = await mediaService.GetUploadUrlAsync(
            User.GetUserId(),
            postId,
            request,
            cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpPost("{postId:guid}/media/{mediaId:guid}/complete")]
    public async Task<IActionResult> CompleteUpload(
        Guid postId,
        Guid mediaId,
        [FromBody] CompleteMediaUploadRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediaService.CompleteUploadAsync(
            User.GetUserId(),
            postId,
            mediaId,
            request,
            cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpGet("media/{mediaId:guid}")]
    public async Task<IActionResult> Get(Guid mediaId, CancellationToken cancellationToken)
    {
        var result = await mediaService.GetAsync(
            User.GetUserId(),
            mediaId,
            cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpDelete("media/{mediaId:guid}")]
    public async Task<IActionResult> Delete(Guid mediaId, CancellationToken cancellationToken)
    {
        var result = await mediaService.DeleteAsync(
            User.GetUserId(),
            mediaId,
            cancellationToken);

        return result.ToApiResult();
    }
}
