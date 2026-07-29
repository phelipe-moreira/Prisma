using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.NgoCause;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers
{
    [ApiController]
    [Route("api/ngo-cause")]
    public class NgoCauseController(INgoCauseService ngoCauseService) : ControllerBase
    {
        [HttpGet("ngo/{ngoId:guid}")]
        public async Task<IActionResult> GetByNgoId(Guid ngoId, CancellationToken cancellationToken)
        {
            var result = await ngoCauseService.GetByNgoIdAsync(ngoId, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpGet("cause/{causeId:guid}")]
        public async Task<IActionResult> GetByCauseId(Guid causeId, CancellationToken cancellationToken)
        {
            var result = await ngoCauseService.GetByCauseIdAsync(causeId, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNgoCauseRequest request, CancellationToken cancellationToken)
        {
            var result = await ngoCauseService.CreateAsync(request, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid ngoId, Guid causeId, CancellationToken cancellationToken)
        {
            await ngoCauseService.DeleteAsync(ngoId, causeId, cancellationToken);

            return NoContent();
        }
    }
}
