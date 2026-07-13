namespace Prisma.Domain.Entities;

public class Ngo
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Cnpj { get; set; }

    public string? Description { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public string? CoverPictureUrl { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? InstagramUrl { get; set; }

    public string? ContactEmail { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }


    public ICollection<NgoCause> NgoCauses { get; set; } = [];

    public ICollection<UserNgo> UserNgos { get; set; } = [];
}
