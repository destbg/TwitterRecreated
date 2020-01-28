using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Common
{
    public class LikedPostRepository : BaseRepository<LikedPost>, ILikedPostRepository
    {
        private readonly IMapper _mapper;

        public LikedPostRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<LikedPost> FindByUserAndPost(long postId, string userId, CancellationToken token) =>
            _context.LikedPosts
                .FirstOrDefaultAsync(f => f.PostId == postId && f.UserId == userId, token);

        public Task<List<PostVm>> UserPosts(string username, DateTime skip, CancellationToken token) =>
            _context.LikedPosts
                .Include(f => f.User)
                .Where(f => f.User.UserName == username && f.LikedOn > skip)
                .Select(f => f.Post)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
    }
}
