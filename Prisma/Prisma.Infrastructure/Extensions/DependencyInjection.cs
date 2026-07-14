using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prisma.Domain.Interfaces;
using Prisma.Domain.Interfaces.Security;
using Prisma.Infrastructure.Auth;
using Prisma.Infrastructure.Context;
using Prisma.Infrastructure.Repositories;
using Prisma.Infrastructure.Security;

namespace Prisma.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtSettings>(
                configuration.GetSection(
                    JwtSettings.SectionName));


            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString(
                        "DefaultConnection"));
            });


            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddJwtAuthentication(configuration);

            return services;
        }
    }
}
