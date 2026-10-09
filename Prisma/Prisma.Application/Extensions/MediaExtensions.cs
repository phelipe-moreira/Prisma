using Prisma.Application.DTOs.Media;
using Prisma.Domain.Entities;

namespace Prisma.Application.Extensions;

public static class MediaExtensions
{
    extension(Media media)
    {
        public MediaResponse ToResponse()
            => new()
            {
                Id = media.Id,
                Type = media.Type,
                StorageKey = media.StorageKey,
                CreatedAt = media.CreatedAt,
            };
    }
}
