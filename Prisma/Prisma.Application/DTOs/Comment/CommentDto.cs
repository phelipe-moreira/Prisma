namespace Prisma.Application.DTOs.Comment;

public record CommentDto(
    Guid PostId,
    Guid? ParentCommentId,
    string Content);
