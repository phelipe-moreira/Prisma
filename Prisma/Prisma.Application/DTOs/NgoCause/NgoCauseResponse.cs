namespace Prisma.Application.DTOs.NgoCause
{
    public class NgoCauseResponse
    {
        public Guid NgoId { get; set; }
        public string NgoName { get; set; } = string.Empty;
        public Guid CauseId { get; set; }
        public string CauseName { get; set; } = string.Empty;
    }
}
