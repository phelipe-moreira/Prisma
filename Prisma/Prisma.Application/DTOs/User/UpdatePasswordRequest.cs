namespace Prisma.Application.DTOs.User
{
    public class UpdatePasswordRequest
    {
        public string CurrentPassword { get; init; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
