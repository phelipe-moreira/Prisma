using Prisma.Application.DTOs.Post;
using Prisma.Domain.Entities;

namespace Prisma.Application.Extensions;

public static class PostExtensions
{
    extension(Post post)
    {
        public PostResponse ToResponse()
            => new()
            {
                Id = post.Id,
                NgoId = post.NgoId,
                Title = post.Title,
                Status = post.Status,
                Content = post.Content,
                Comments = post.Comments,
                Likes = post.PostLikes.Count,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            };
    }
}