using System;
using System.Linq;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Common;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = (
                Environment.GetEnvironmentVariable("DATABASE_URL")
                ?? configuration.GetConnectionString("DefaultDatabase")
            ).Split('/').Last().Split(':');

            services.AddDbContext<TwitterDbContext>(options =>
                    options.UseNpgsql($"Server=ec2-54-246-89-234.eu-west-1.compute.amazonaws.com;Port=5432;Database=dfcr9qvegt6bq4;User Id={connectionString[0]};Password={connectionString[1]};")
                );

            services.AddScoped<ITwitterDbContext>(provider => provider.GetService<TwitterDbContext>());

            services.AddScoped<ILikedPostRepository, LikedPostRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUserFollowRepository, UserFollowRepository>();
            services.AddScoped<IHashTagRepository, HashTagRepository>();
            services.AddScoped<IBookmarkRepository, BookmarkRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IPollOptionRepository, PollOptionRepository>();
            services.AddScoped<IPollVoteRepository, PollVoteRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatUserRepository, ChatUserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IRepostRepository, RepostRepository>();

            return services;
        }
    }
}
