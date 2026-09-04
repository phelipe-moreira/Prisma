using Prisma.Domain.Enums;

namespace Prisma.Application.DTOs.User
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; init; } = string.Empty;
        public required string Email { get; init; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserRole Role { get; set; }
        public ICollection<Prisma.Application.DTOs.Post.PostResponse> SavedPosts { get; set; } = [];
    }
}
