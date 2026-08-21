using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers
{
    [ApiController]
    [Route("api/post")]
    [Authorize]
    public class SavedPostController(ISavedPostService savedPostService) : ControllerBase
    {
        [HttpPost("{postId:guid}/save")]
        public async Task<IActionResult> Save(Guid postId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await savedPostService.SaveAsync(userId, postId, cancellationToken);

            return result.ToApiResult();
        }

        [HttpDelete("{postId:guid}/save")]
        public async Task<IActionResult> Unsave(Guid postId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await savedPostService.UnsaveAsync(userId, postId, cancellationToken);

            return result.ToApiResult();
        }
    }
}
