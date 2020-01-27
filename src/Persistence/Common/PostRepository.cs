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
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly IMapper _mapper;

        public PostRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> VerifyPosts(long[] postIds, CancellationToken token) =>
            await _context.Posts.CountAsync(f => postIds.Contains(f.Id), token) == postIds.Length;

        public async Task<IEnumerable<PostVm>> FindPostsFromUsers(DateTime skip, CancellationToken token) =>
            await _context.Posts
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .Where(f => f.PostedOn > skip)
                .OrderByDescending(f => f.PostedOn)
                .Take(50)
                .ToListAsync(token);

        public async Task<PostVm> FindById(long id, CancellationToken token) =>
            await _context.Posts
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(f => f.Id == id, token);

        public async Task<IEnumerable<PostVm>> UserPosts(DateTime skip, string username, CancellationToken token) =>
            await _context.Posts
                .Include(f => f.User)
                .Where(f => f.PostedOn > skip && f.User.UserName == username)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .Take(50)
                .ToListAsync(token);
    }
}
