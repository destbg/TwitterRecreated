using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common
{
    public class UserFollowRepository : GenericRepository<UserFollow>, IUserFollowRepository
    {
        private readonly IMapper _mapper;

        public UserFollowRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<UserShortVm>> FollowingFollowers(string userId, CancellationToken token) =>
            await _context.UserFollowers
                .Where(f => f.FollowerId == userId)
                .Select(f => f.Following)
                .ProjectTo<UserShortVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

        public async Task<IReadOnlyList<string>> FollowingUsers(string userId, CancellationToken token) =>
            await _context.UserFollowers
                .Where(w => w.FollowerId == userId)
                .Select(f => f.FollowingId)
                .ToListAsync(token);

        public async Task<IEnumerable<UserFollow>> FollowingAndFollowers(string userId, CancellationToken token) =>
            await _context.UserFollowers
                .Where(f => f.FollowerId == userId || f.FollowingId == userId)
                .ToListAsync(token);

        public async Task<IEnumerable<UserShortVm>> Suggestions(string userId, CancellationToken token) =>
            await _context.UserFollowers
                .Where(f => f.FollowerId != userId && f.FollowingId != userId)
                .Select(f => f.Following)
                .OrderByDescending(f => f.Followers)
                .ProjectTo<UserShortVm>(_mapper.ConfigurationProvider)
                .Take(3)
                .ToListAsync(token);
    }
}
