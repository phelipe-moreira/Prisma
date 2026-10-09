using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.Comment;
using Prisma.Application.Interfaces;

namespace Prisma.Api.Controllers;

[ApiController]
[Route("api/comment")]
[Produces("application/json")]
public class CommentController(ICommentService commentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await commentService.GetAllAsync(cancellationToken);

        return result.ToApiResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await commentService.GetByIdAsync(id, cancellationToken);

        return result.ToApiResult();
    }

    [HttpGet("parent/{parentId:guid}")]
    public async Task<IActionResult> GetByParentId(Guid parentId, CancellationToken cancellationToken)
    {
        var result = await commentService.GetByParentIdAsync(parentId, cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommentDto commentDto, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await commentService.CreateAsync(userId, commentDto, cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentDto updateCommentDto, CancellationToken cancellationToken)
    {
        var result = await commentService.UpdateAsync(id, updateCommentDto, cancellationToken);

        return result.ToApiResult();
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remove(Guid id, CancellationToken cancellationToken)
    {
        var result = await commentService.RemoveAsync(id, cancellationToken);

        return result.ToApiResult();
    }
}
