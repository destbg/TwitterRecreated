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
            services.AddDbContext<TwitterDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultDatabase"))
                );

            services.AddScoped<ITwitterDbContext>(provider => provider.GetService<TwitterDbContext>());

            services.AddTransient<ILikedPostRepository, LikedPostRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IUserFollowRepository, UserFollowRepository>();

            return services;
        }
    }
}
