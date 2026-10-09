using Prisma.Domain.Enums;

namespace Prisma.Domain.Entities;

public class UserNgo
{
    public Guid UserId { get; set; }

    public Guid NgoId { get; set; }

    public UserNgoRole Role { get; set; }

    public DateTime CreatedAt { get; set; }


    public User User { get; set; } = null!;

    public Ngo Ngo { get; set; } = null!;

    private UserNgo()
    {
    }

    public static UserNgo Create(Guid userId, Guid ngoId, UserNgoRole role)
    {
        return new()
        {
            UserId = userId,
            NgoId = ngoId,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };
    }
}
