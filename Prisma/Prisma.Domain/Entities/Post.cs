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


    public Ngo Ngo { get; set; } = null!;

    public ICollection<PostLike> PostLikes { get; set; } = [];

    public ICollection<PostMedia> PostMedias { get; set; } = [];

    public ICollection<SavedPost> SavedPosts { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];

    private Post() { Id = Guid.NewGuid(); }

    public static Post Create(
        Guid ngoId, 
        string title, 
        string? content, 
        PostStatus status)
        => new()
        {
            Title = title,
            NgoId = ngoId,
            Content = content,
            Status = status,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

    public void Update(string title, string? content, PostStatus status)
    {
        Title = title;
        Content = content;
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}

