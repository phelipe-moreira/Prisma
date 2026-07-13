namespace Prisma.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public string? Bio { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }


    public ICollection<PostLike> PostLikes { get; set; } = [];

    public ICollection<SavedPost> SavedPosts { get; set; } = [];

    public ICollection<UserNgo> UserNgos { get; set; } = [];

    public ICollection<UserNgoFollow> UserNgoFollows { get; set; } = [];

    public ICollection<MediaList> MediaLists { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];
}
