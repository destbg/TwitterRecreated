using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public async Task Initialize(ClaimsPrincipal user, ConnectionInfo connection, IUserManager userManager)
        {
            if (user?.Identity?.IsAuthenticated == true)
            {
                User = await userManager.GetCurrentUser(user.Identity.Name)
                    ?? throw new UnauthorizedAccessException("Provided credentials are invalid");
                IsAuthenticated = true;
            }
            Ip = connection?.RemoteIpAddress?.ToString();
        }

        public string Ip { get; private set; }

        public AppUser User { get; private set; }

        public bool IsAuthenticated { get; private set; }
    }
}
