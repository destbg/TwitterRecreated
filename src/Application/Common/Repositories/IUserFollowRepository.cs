using Application.Common.ViewModels;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Repositories
{
    public interface IUserFollowRepository : IRepository<UserFollow>
    {
        Task<List<UserShortVm>> FollowingFollowers(string userId, CancellationToken token);
        Task<List<string>> FollowingUsers(string userId, CancellationToken token);
        Task<List<UserFollow>> FollowingAndFollowers(string userId, CancellationToken token);
        Task<List<UserShortVm>> Suggestions(string userId, CancellationToken token);
    }
}
