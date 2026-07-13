namespace Prisma.Domain.Entities;

public class Cause
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }


    public ICollection<NgoCause> NgoCauses { get; set; } = [];
}
