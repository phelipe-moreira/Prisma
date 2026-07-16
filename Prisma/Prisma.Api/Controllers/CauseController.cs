using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.Cause;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers
{
    [ApiController]
    [Route("api/cause")]
    public class CauseController : ControllerBase
    {
        private readonly ICauseService _causeService;

        public CauseController(ICauseService causeService)
        {
            _causeService = causeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _causeService.GetAllAsync(cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _causeService.GetByIdAsync(id, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCauseRequest request, CancellationToken cancellationToken)
        {
            var result = await _causeService.CreateAsync(request, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateCauseRequest request, CancellationToken cancellationToken)
        {
            var result = await _causeService.UpdateAsync(id, request, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _causeService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }

    }
}
