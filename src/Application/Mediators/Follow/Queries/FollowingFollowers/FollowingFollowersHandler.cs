using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Follow.Queries.FollowingFollowers
{
    public class FollowingFollowersHandler : IRequestHandler<FollowingFollowersQuery, IEnumerable<UserShortVm>>
    {
        private readonly IUserFollowRepository _userFollow;
        private readonly ICurrentUserService _currentUser;

        public FollowingFollowersHandler(IUserFollowRepository userFollow, ICurrentUserService currentUser)
        {
            _userFollow = userFollow ?? throw new ArgumentNullException(nameof(userFollow));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<UserShortVm>> Handle(FollowingFollowersQuery request, CancellationToken cancellationToken) =>
            await _userFollow.FollowingFollowers(_currentUser.UserId, cancellationToken);
    }
}
