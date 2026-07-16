namespace Prisma.Application.DTOs.Cause
{
    public class CauseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
