using Application.Common.ViewModels;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Repositories
{
    public interface IUserFollowRepository : IRepository<UserFollow>
    {
        Task<IEnumerable<UserShortVm>> FollowingFollowers(string userId, CancellationToken token);
        Task<IReadOnlyList<string>> FollowingUsers(string userId, CancellationToken token);
        Task<IEnumerable<UserFollow>> FollowingAndFollowers(string userId, CancellationToken token);
        Task<IEnumerable<UserShortVm>> Suggestions(string userId, CancellationToken token);
    }
}
