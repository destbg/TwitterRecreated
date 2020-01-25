using Application.Common.Interfaces;
using Application.Common.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Follow.Queries.FollowingFollowers
{
    public class FollowingFollowersHandler : IRequestHandler<FollowingFollowersQuery, IEnumerable<UserShortVm>>
    {
        private readonly IRepository<UserFollow> _userFollow;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public FollowingFollowersHandler(IRepository<UserFollow> userFollow, IMapper mapper, ICurrentUserService currentUser)
        {
            _userFollow = userFollow ?? throw new ArgumentNullException(nameof(userFollow));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<UserShortVm>> Handle(FollowingFollowersQuery request, CancellationToken cancellationToken) =>
            await _userFollow.GetAll()
                .Where(f => f.FollowerId == _currentUser.UserId)
                .Join(_userFollow.GetSet(), f => f.FollowingId, f => f.FollowerId, (_, f) => f)
                .Select(f => f.Following)
                .ProjectTo<UserShortVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
    }
}
