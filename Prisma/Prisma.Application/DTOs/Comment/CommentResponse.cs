namespace Prisma.Application.DTOs.Comment;

public record CommentResponse(
    Guid Id,
    Guid PostId,
    Guid UserId,
    Guid? ParentCommentId,
    string Content,
    DateTime CreatedAt);
