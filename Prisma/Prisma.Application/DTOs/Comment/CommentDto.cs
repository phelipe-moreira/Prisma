namespace Prisma.Application.DTOs.Comment;

public record CommentDto(
    Guid PostId,
    Guid UserId,
    Guid? ParentCommentId,
    string Content);
