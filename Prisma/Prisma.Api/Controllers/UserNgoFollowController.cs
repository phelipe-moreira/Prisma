using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.UserNgoFollow;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers
{
    [ApiController]
    [Route("api/user-ngo-follow")]
    public class UserNgoFollowController(IUserNgoFollowService userNgoFollowService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserNgoFollowRequest request, CancellationToken cancellationToken)
        {
            var result = await userNgoFollowService.CreateAsync(request, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var result = await userNgoFollowService.GetByUserIdAsync(userId, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpGet("ngo/{ngoId:guid}")]
        public async Task<IActionResult> GetByNgoId(Guid ngoId, CancellationToken cancellationToken)
        {
            var result = await userNgoFollowService.GetByNgoIdAsync(ngoId, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpDelete("{userId:guid}/{ngoId:guid}")]
        public async Task<IActionResult> Delete(Guid userId, Guid ngoId, CancellationToken cancellationToken)
        {
            var result = await userNgoFollowService.DeleteAsync(userId, ngoId, cancellationToken);

            return result.ToApiResult(this);
        }
    }
}
