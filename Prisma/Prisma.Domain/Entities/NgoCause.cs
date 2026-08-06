namespace Prisma.Domain.Entities;

public class NgoCause
{
    public Guid NgoId { get; set; }
    public Guid CauseId { get; set; }

    public Ngo Ngo { get; set; } = null!;
    public Cause Cause { get; set; } = null!;

    private NgoCause()
    {
    }

    public static NgoCause Create(Guid ngoId, Guid causeId)
    {
        return new NgoCause
        {
            NgoId = ngoId,
            CauseId = causeId
        };
    }
}
