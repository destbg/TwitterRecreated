using System;
using System.Text;
using Application.Common.Interfaces;
using CloudinaryDotNet;
using Common;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var account = new Account(
                configuration.GetValue<string>("Cloudinary:Name"),
                configuration.GetValue<string>("Cloudinary:Api_Key"),
                configuration.GetValue<string>("Cloudinary:Api_Secret")
            );
            var cloudinary = new Cloudinary(account);

            services.AddSingleton(_ => cloudinary);
            services.AddScoped<IUserManager, UserManagerService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IDateTime, UniversalDateTime>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IVideoService, VideoService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddSingleton<IConnectionMapping, ConnectionMapping>();
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();

            services.AddDefaultIdentity<AppUser>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequiredLength = 12;
                opt.Password.RequiredUniqueChars = 0;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = true;
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<TwitterDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<AppUser, TwitterDbContext>();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddIdentityServerJwt()
                .AddJwtBearer(token =>
                {
                    token.RequireHttpsMetadata = false;
                    token.SaveToken = true;
                    token.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}
