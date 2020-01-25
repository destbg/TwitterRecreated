using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Follow.Queries.FollowingUsers
{
    public class FollowingUsersHandler : IRequestHandler<FollowingUsersQuery, IEnumerable<string>>
    {
        private readonly IRepository<UserFollow> _userFollow;
        private readonly ICurrentUserService _currentUser;

        public FollowingUsersHandler(IRepository<UserFollow> userFollow, ICurrentUserService currentUser)
        {
            _userFollow = userFollow ?? throw new ArgumentNullException(nameof(userFollow));
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<string>> Handle(FollowingUsersQuery request, CancellationToken cancellationToken) =>
            await _userFollow.GetAll()
                .Where(w => w.FollowingId == _currentUser.UserId)
                .Select(f => f.FollowingId)
                .ToListAsync(cancellationToken);
    }
}
