using Application.Common.Interfaces;
using Application.Common.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Follow.Queries.FollowingUsers
{
    public class FollowingUsersHandler : IRequestHandler<FollowingUsersQuery, IReadOnlyList<string>>
    {
        private readonly IUserFollowRepository _userFollow;
        private readonly ICurrentUserService _currentUser;

        public FollowingUsersHandler(IUserFollowRepository userFollow, ICurrentUserService currentUser)
        {
            _userFollow = userFollow ?? throw new ArgumentNullException(nameof(userFollow));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IReadOnlyList<string>> Handle(FollowingUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _userFollow.FollowingUsers(_currentUser.UserId, cancellationToken);
            result.Add(_currentUser.UserId);
            return result;
        }
    }
}
