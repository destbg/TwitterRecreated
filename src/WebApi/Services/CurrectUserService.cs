using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            IsAuthenticated = UserId != null;
            Ip = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }

        public string Ip { get; }

        public string UserId { get; }

        public bool IsAuthenticated { get; }
    }
}
