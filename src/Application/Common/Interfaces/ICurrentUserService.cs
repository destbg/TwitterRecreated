using System.Security.Claims;
using System.Threading.Tasks;
using Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string Ip { get; }
        AppUser User { get; }
        bool IsAuthenticated { get; }
        Task Initialize(ClaimsPrincipal user, ConnectionInfo connection, IUserManager userManager, IMemoryCacheService cacheService);
    }
}
