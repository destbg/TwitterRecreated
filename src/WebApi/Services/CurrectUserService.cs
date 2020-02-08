using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ILogger<CurrentUserService> _logger;

        public CurrentUserService(ILogger<CurrentUserService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Initialize(ClaimsPrincipal user, ConnectionInfo connection, IUserManager userManager)
        {
            if (user?.Identity?.IsAuthenticated == true)
            {
                User = await userManager.GetCurrentUser(user.Identity.Name)
                    ?? throw new UnauthorizedAccessException("Provided credentials are invalid");

                IsAuthenticated = true;
            }
            Ip = connection?.RemoteIpAddress?.ToString();
            _logger.LogInformation(Ip);
        }

        public string Ip { get; private set; }

        public AppUser User { get; private set; }

        public bool IsAuthenticated { get; private set; }
    }
}
