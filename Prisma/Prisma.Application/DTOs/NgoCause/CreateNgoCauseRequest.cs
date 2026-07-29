namespace Prisma.Application.DTOs.NgoCause
{
    public class CreateNgoCauseRequest
    {
        public Guid NgoId { get; set; }
        public Guid CauseId { get; set; }
    }
}
