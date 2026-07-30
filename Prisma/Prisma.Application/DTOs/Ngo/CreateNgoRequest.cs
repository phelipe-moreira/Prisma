namespace Prisma.Application.DTOs.Ngo
{
    public class CreateNgoRequest
    {
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
    }
}
