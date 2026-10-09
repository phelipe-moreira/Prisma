namespace Prisma.Application.DTOs.Cause
{
    public class CreateCauseRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
