using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.UserNgoFollow;
using Prisma.Application.Interfaces;
using System.Security.Claims;

namespace Prisma.Api.Controllers
{
    [ApiController]
    [Route("api/user-ngo-follow")]
    public class UserNgoFollowController(IUserNgoFollowService userNgoFollowService) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserNgoFollowRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await userNgoFollowService.CreateAsync(userId, request, cancellationToken);

            return result.ToApiResult();
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyFollows(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await userNgoFollowService.GetByUserIdAsync(
                userId,
                cancellationToken);

            return result.ToApiResult();
        }

        [HttpGet("ngo/{ngoId:guid}")]
        public async Task<IActionResult> GetByNgoId(Guid ngoId, CancellationToken cancellationToken)
        {
            var result = await userNgoFollowService.GetByNgoIdAsync(ngoId, cancellationToken);

            return result.ToApiResult();
        }

        [Authorize]
        [HttpDelete("{ngoId:guid}")]
        public async Task<IActionResult> Delete(Guid ngoId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await userNgoFollowService.DeleteAsync(userId, ngoId, cancellationToken);

            return result.ToApiResult();
        }
    }
}
