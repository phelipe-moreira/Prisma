using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.UserNgo;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers
{
    [ApiController]
    [Route("api/user-ngo")]
    public class UserNgoController(IUserNgoService userNgoService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserNgoRequest request, CancellationToken cancellationToken)
        {
            var result = await userNgoService.CreateAsync(request, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var result = await userNgoService.GetByUserIdAsync(userId, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpGet("ngo/{ngoId:guid}")]
        public async Task<IActionResult> GetByNgoId(Guid ngoId, CancellationToken cancellationToken)
        {
            var result = await userNgoService.GetByNgoIdAsync(ngoId, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpDelete("{userId:guid}/{ngoId:guid}")]
        public async Task<IActionResult> Delete(Guid userId, Guid ngoId, CancellationToken cancellationToken)
        {
            await userNgoService.DeleteAsync(userId, ngoId, cancellationToken);

            return NoContent();
        }
    }
}
