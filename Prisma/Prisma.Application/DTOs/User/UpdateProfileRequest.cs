namespace Prisma.Application.DTOs.User
{
    public class UpdateProfileRequest
    {
        public required string Name { get; init; } = string.Empty;
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
