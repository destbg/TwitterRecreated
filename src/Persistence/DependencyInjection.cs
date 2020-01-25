using Application.Common.Interfaces;
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

            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
