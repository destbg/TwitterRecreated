using Application.Common.ViewModels;
using Domain.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Repositories
{
    public interface IUserFollowRepository : IRepository<UserFollow>
    {
        Task<List<UserShortVm>> FollowingFollowers(IEnumerable<string> userIds, string selfId, CancellationToken token);
        Task<List<string>> FollowingUsers(string userId, CancellationToken token);
        Task<List<string>> FollowingUsernames(IEnumerable<string> usernames, string userId, CancellationToken token);
        Task<List<UserFollow>> FollowingAndFollowers(string userId, CancellationToken token);
        Task<List<UserShortVm>> Suggestions(IEnumerable<string> userIds, string userId, CancellationToken token);
    }
}
