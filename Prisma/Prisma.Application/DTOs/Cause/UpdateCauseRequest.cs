namespace Prisma.Application.DTOs.Cause
{
    public class UpdateCauseRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
