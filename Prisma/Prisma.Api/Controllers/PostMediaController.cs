using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.Post;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers;

[ApiController]
[Route("api/post")]
public class PostMediaController(IPostMediaService postMediaService) : ControllerBase
{
    [HttpPost("{postId:guid}/media")]
    public async Task<IActionResult> Add(Guid postId, PostMediaDto request, CancellationToken cancellationToken)
    {
        var result = await postMediaService.AddAsync(postId, request, cancellationToken);

        return result.ToApiResult();
    }

    [HttpDelete("{postId:guid}/media/{mediaId:guid}")]
    public async Task<IActionResult> Remove(Guid postId, Guid mediaId, CancellationToken cancellationToken)
    {
        var result = await postMediaService.RemoveAsync(postId, mediaId, cancellationToken);

        return result.ToApiResult();
    }
}
