using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.User;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await userService.GetByIdAsync(userId, cancellationToken);

            return result.ToApiResult();
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await userService.UpdateProfileAsync(userId, request, cancellationToken);

            return result.ToApiResult();
        }

        [HttpPut("me/password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await userService.UpdatePassowdAsync(userId, request, cancellationToken);

            return result.ToApiResult();
        }

        [HttpDelete("me")]
        public async Task<IActionResult> Deactivate(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await userService.DeactivateAsync(userId, cancellationToken);

            return result.ToApiResult();
        }
    }
}
