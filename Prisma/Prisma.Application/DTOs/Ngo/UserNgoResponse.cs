using Prisma.Domain.Enums;

namespace Prisma.Application.DTOs.Ngo;

public class UserNgoResponse
{
    public Guid UserId { get; set; }

    public string? Name { get; set; }

    public UserNgoRole Role { get; set; }
}
