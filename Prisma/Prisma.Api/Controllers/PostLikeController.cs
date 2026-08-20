using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers
{
    [ApiController]
    [Route("api/post")]
    [Authorize]
    public class PostLikeController(IPostLikeService postLikeService) : ControllerBase
    {
        [HttpPost("{postId:guid}/like")]
        public async Task<IActionResult> Like(Guid postId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await postLikeService.LikeAsync(userId, postId, cancellationToken);

            return result.ToApiResult();
        }

        [HttpDelete("{postId:guid}/like")]
        public async Task<IActionResult> Unlike(Guid postId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await postLikeService.UnlikeAsync(userId, postId, cancellationToken);

            return result.ToApiResult();
        }
    }
}
