using Prisma.Application.DTOs.Post;
using Prisma.Domain.Entities;

namespace Prisma.Application.Extensions;

public static class PostExtensions
{
    extension(Post post)
    {
        public PostResponse ToResponse()
            => post.ToResponse(storageService: null);

        public PostResponse ToResponse(Prisma.Application.Interfaces.IStorageService? storageService, Guid? currentUserId = null)
            => new()
            {
                Id = post.Id,
                NgoId = post.NgoId,
                Title = post.Title,
                Status = post.Status,
                Content = post.Content,
                Comments = [.. post.Comments
                    .Select(comment => new DTOs.Comment.CommentResponse(
                        comment.Id,
                        comment.PostId,
                        comment.UserId,
                        comment.ParentCommentId,
                        comment.Content,
                        comment.CreatedAt,
                        comment.IsRemoved))],
                Likes = post.PostLikes.Count,
                IsLikedByCurrentUser = currentUserId.HasValue && post.PostLikes.Any(like => like.UserId == currentUserId.Value),
                Medias = [.. post.PostMedias
                    .OrderBy(postMedia => postMedia.DisplayOrder)
                    .Select(postMedia => new Prisma.Application.DTOs.Media.MediaResponse
                    {
                        Id = postMedia.Media.Id,
                        Type = postMedia.Media.Type,
                        StorageKey = postMedia.Media.StorageKey,
                        Url = storageService?.GetPublicUrl(postMedia.Media.StorageKey),
                        CreatedAt = postMedia.Media.CreatedAt
                    })],
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            };
    }
}
