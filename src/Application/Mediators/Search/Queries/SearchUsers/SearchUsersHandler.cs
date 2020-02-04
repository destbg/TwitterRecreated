using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchUsers
{
    public class SearchUsersHandler : IRequestHandler<SearchUsersQuery, IEnumerable<UserFollowVm>>
    {
        private readonly IUserManager _userManager;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserFollowRepository _userFollow;

        public SearchUsersHandler(IUserManager userManager, ICurrentUserService currentUser, IUserFollowRepository userFollow)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _userFollow = userFollow ?? throw new ArgumentNullException(nameof(userFollow));
        }

        public async Task<IEnumerable<UserFollowVm>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            var results = await _userManager.SeachUsers(request.Search);
            var followingUsers = await _userFollow.FollowingUsernames(results.Select(f => f.Username), _currentUser.User.Id, cancellationToken);

            return results.Select(f =>
            {
                f.Followed = followingUsers.Contains(f.Username);
                return f;
            });
        }
    }
}
