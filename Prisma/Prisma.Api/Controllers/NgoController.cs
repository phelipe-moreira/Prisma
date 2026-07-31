using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.Ngo;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers
{
    [ApiController]
    [Route("api/ngo")]
    public class NgoController(INgoService ngoService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await ngoService.GetAllAsync(cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await ngoService.GetByIdAsync(id, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNgoRequest request, CancellationToken cancellationToken)
        {
            var result = await ngoService.CreateAsync(request, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateNgoRequest request, CancellationToken cancellationToken)
        {
            var result = await ngoService.UpdateAsync(id, request, cancellationToken);

            return result.ToApiResult(this);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await ngoService.DeleteAsync(id, cancellationToken);

            return result.ToApiResult(this);
        }
    }
}
