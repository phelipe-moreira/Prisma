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


    public ICollection<Post> Posts { get; set; } = [];

    public ICollection<NgoCause> NgoCauses { get; set; } = [];

    public ICollection<UserNgo> UserNgos { get; set; } = [];

    public ICollection<UserNgoFollow> UserNgoFollows { get; set; } = [];

    private Ngo()
    {
    }

    public static Ngo Create(
        string name,
        string cnpj,
        string? description,
        string? profilePictureUrl,
        string? coverPictureUrl,
        string? websiteUrl,
        string? instagramUrl,
        string? contactEmail,
        string? city,
        string? state)
    {
        return new Ngo
        {
            Id = Guid.NewGuid(),
            Name = name,
            Cnpj = cnpj,
            Description = description,
            ProfilePictureUrl = profilePictureUrl,
            CoverPictureUrl = coverPictureUrl,
            WebsiteUrl = websiteUrl,
            InstagramUrl = instagramUrl,
            ContactEmail = contactEmail,
            City = city,
            State = state,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void Update(
        string name,
        string? description,
        string? profilePictureUrl,
        string? coverPictureUrl,
        string? websiteUrl,
        string? instagramUrl,
        string? contactEmail,
        string? city,
        string? state)
    {
        Name = name;
        Description = description;
        ProfilePictureUrl = profilePictureUrl;
        CoverPictureUrl = coverPictureUrl;
        WebsiteUrl = websiteUrl;
        InstagramUrl = instagramUrl;
        ContactEmail = contactEmail;
        City = city;
        State = state;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
