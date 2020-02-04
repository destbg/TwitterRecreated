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
    public class UserFollowRepository : BaseRepository<UserFollow>, IUserFollowRepository
    {
        private readonly IMapper _mapper;

        public UserFollowRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<List<UserShortVm>> FollowingFollowers(IEnumerable<string> userIds, string selfId, CancellationToken token) =>
            Query.Where(f => userIds.Any(a => a == f.FollowerId) && f.FollowingId == selfId)
                .Select(f => f.Follower)
                .ProjectTo<UserShortVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

        public Task<List<string>> FollowingUsers(string userId, CancellationToken token) =>
            Query.Where(w => w.FollowerId == userId)
                .Select(f => f.FollowingId)
                .ToListAsync(token);

        public Task<List<UserFollow>> FollowingAndFollowers(string userId, CancellationToken token) =>
            Query.Where(f => f.FollowerId == userId || f.FollowingId == userId)
                .ToListAsync(token);

        public Task<List<UserShortVm>> Suggestions(IEnumerable<string> userIds, string userId, CancellationToken token) =>
            Query.Include(f => f.Following)
                .Where(f => !userIds.Contains(f.FollowerId) && f.FollowingId != userId)
                .OrderByDescending(f => f.Following.Followers)
                .Select(f => f.Follower)
                .Take(3)
                .ProjectTo<UserShortVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

        public Task<List<string>> FollowingUsernames(IEnumerable<string> usernames, string userId, CancellationToken token) =>
            Query.Include(f => f.Following)
                .Where(f => usernames.Contains(f.Following.UserName) && f.FollowerId == userId)
                .Select(f => f.Following.UserName)
                .ToListAsync(token);
    }
}
