using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prisma.Application.Interfaces;
using Prisma.Application.Services;
using Prisma.Domain.Abstractions;
using Prisma.Infrastructure.Auth;
using Prisma.Infrastructure.Context;
using Prisma.Infrastructure.Repositories;
using Prisma.Infrastructure.Security;
using Prisma.Infrastructure.Storage;

namespace Prisma.Infrastructure.Extensions;

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

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddJwtAuthentication(configuration);

        services.Configure<CloudflareR2Options>(configuration.GetSection("CloudflareR2"));

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var options = sp
                .GetRequiredService<IOptions<CloudflareR2Options>>()
                .Value;

            var config = new AmazonS3Config
            {
                ServiceURL = $"https://{options.AccountId}.r2.cloudflarestorage.com",
                AuthenticationRegion = "auto"
            };

            return new AmazonS3Client(
                options.AccessKeyId,
                options.SecretAccessKey,
                config);
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICauseRepository, CauseRepository>();
        services.AddScoped<ICauseService, CauseService>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<INgoRepository, NgoRepository>();
        services.AddScoped<INgoService, NgoService>();
        services.AddScoped<IUserNgoFollowService, UserNgoFollowService>();
        services.AddScoped<INgoAccessService, NgoAccessService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IPostLikeRepository, PostLikeRepository>();
        services.AddScoped<IPostLikeService, PostLikeService>();
        services.AddScoped<ISavedPostService, SavedPostService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStorageService, CloudflareR2StorageService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<INgoAccessService, NgoAccessService>();

        return services;
    }
}
