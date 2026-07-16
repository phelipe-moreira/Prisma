namespace Prisma.Domain.Entities;

public class Cause
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<NgoCause> NgoCauses { get; set; } = [];

    private Cause() { }

    public static Cause Create(string name, string? description)
    {
        return new Cause
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
