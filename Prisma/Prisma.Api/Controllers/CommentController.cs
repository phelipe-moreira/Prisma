using Microsoft.AspNetCore.Mvc;
using Prisma.Api.Extensions;
using Prisma.Application.DTOs.Comment;
using Prisma.Application.Interfaces;
using Prisma.Domain.Entities;

namespace Prisma.Api.Controllers;

[ApiController]
[Route("api/comment")]
[Produces("application/json")]
public class CommentController(ICommentService commentService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CommentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await commentService.GetAllAsync(cancellationToken);

        return result.ToApiResult(this);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await commentService.GetByIdAsync(id, cancellationToken);

        return result.ToApiResult(this);
    }

    [HttpGet("parent/{parentId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CommentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByParentId(Guid parentId, CancellationToken cancellationToken)
    {
        var result = await commentService.GetByParentIdAsync(parentId, cancellationToken);

        return result.ToApiResult(this);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CommentDto commentDto, CancellationToken cancellationToken)
    {
        var result = await commentService.CreateAsync(commentDto, cancellationToken);

        return result.ToApiResult(this);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentDto updateCommentDto, CancellationToken cancellationToken)
    {
        var result = await commentService.UpdateAsync(id, updateCommentDto, cancellationToken);

        return result.ToApiResult(this);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(Guid id, CancellationToken cancellationToken)
    {
        var result = await commentService.RemoveAsync(id, cancellationToken);

        return result.ToApiResult(this);
    }
}
