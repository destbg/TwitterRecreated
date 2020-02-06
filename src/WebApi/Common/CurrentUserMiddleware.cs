using System;
using System.Net;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebApi.Common
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ICurrentUserService currentUser, IUserManager userManager, IMemoryCacheService cacheService)
        {
            if (!httpContext.Request.Path.StartsWithSegments("/main", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    await currentUser.Initialize(httpContext.User, httpContext.Connection, userManager, cacheService);
                }
                catch (UnauthorizedAccessException ex)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsync(ex.Message);
                    return;
                }
            }
            await _next(httpContext);
        }
    }

    public static class CurrentUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseCurrentUserMiddleware(this IApplicationBuilder builder) =>
            builder.UseMiddleware<CurrentUserMiddleware>();
    }
}
