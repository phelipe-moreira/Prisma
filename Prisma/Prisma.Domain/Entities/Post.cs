using Prisma.Domain.Enums;

namespace Prisma.Domain.Entities;

public class Post
{
    public Guid Id { get; set; }

    public Guid NgoId { get; set; }

    public required string Title { get; set; }

    public string? Content { get; set; }

    public PostStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; } 


    public ICollection<PostLike> PostLikes { get; set; } = [];

    public ICollection<PostLink> PostLinks { get; set; } = [];

    public ICollection<PostMedia> PostMedias { get; set; } = [];

    public ICollection<SavedPost> SavedPosts { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];
}

