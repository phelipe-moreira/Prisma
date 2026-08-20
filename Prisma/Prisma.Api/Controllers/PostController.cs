using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.Ngo;
using Prisma.Application.DTOs.Post;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers;

[ApiController]
[Route("api/post")]
public class PostController(IPostService postService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await postService.GetAllAsync(cancellationToken);

        return result.ToApiResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await postService.GetByIdAsync(id, cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreatePostDto request, CancellationToken cancellationToken)
    {
        var result = await postService.CreateAsync(request, cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdatePostDto request, CancellationToken cancellationToken)
    {
        var result = await postService.UpdateAsync(id, request, cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await postService.RemoveAsync(id, cancellationToken);

        return result.ToApiResult();
    }
}
