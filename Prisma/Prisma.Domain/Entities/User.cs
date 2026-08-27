using Prisma.Domain.Enums;

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

    public UserRole Role { get; set; }


    public ICollection<PostLike> PostLikes { get; set; } = [];

    public ICollection<SavedPost> SavedPosts { get; set; } = [];

    public ICollection<UserNgo> UserNgos { get; set; } = [];

    public ICollection<UserNgoFollow> UserNgoFollows { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];

    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

    private User()
    {
    }

    public static User Create(
        string name,
        string email,
        string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            Role = UserRole.User,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(
        string name,
        string? bio,
        string? profilePictureUrl)
    {
        Name = name;
        Bio = bio;
        ProfilePictureUrl = profilePictureUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
