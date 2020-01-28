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
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly IMapper _mapper;

        public PostRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<int> VerifyPosts(long[] postIds, CancellationToken token) =>
            _context.Posts.CountAsync(f => postIds.Contains(f.Id), token);

        public Task<List<PostVm>> FindPostsFromUsers(DateTime skip, CancellationToken token) =>
            _context.Posts
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .Where(f => f.PostedOn > skip)
                .OrderByDescending(f => f.PostedOn)
                .Take(50)
                .ToListAsync(token);

        public Task<PostVm> FindById(long id, CancellationToken token) =>
            _context.Posts
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(f => f.Id == id, token);

        public Task<List<PostVm>> UserPosts(DateTime skip, string username, CancellationToken token) =>
            _context.Posts
                .Include(f => f.User)
                .Where(f => f.PostedOn > skip && f.User.UserName == username)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .OrderByDescending(f => f.PostedOn)
                .Take(50)
                .ToListAsync(token);

        public Task<List<PostVm>> MultimediaPosts(string username, DateTime skip, CancellationToken token) =>
            _context.Posts
                .Include(f => f.Images)
                .Include(f => f.User)
                .Where(f => (f.Video != null || f.Images.Count > 0) && f.User.UserName == username && f.PostedOn > skip)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .OrderByDescending(f => f.PostedOn)
                .Take(50)
                .ToListAsync(token);

        public Task<List<PostVm>> SearchImagePosts(string search, DateTime skip, CancellationToken token) =>
            _context.Posts
                .Include(f => f.Images)
                .Where(f => f.Images.Count > 0 && EF.Functions.Like(f.Content, '%' + search + '%') && f.PostedOn > skip)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .OrderByDescending(f => f.Likes + f.Comments + f.Reposts)
                .ThenByDescending(f => f.PostedOn)
                .Take(50)
                .ToListAsync(token);

        public Task<List<PostVm>> SearchPosts(string search, DateTime skip, CancellationToken token) =>
            _context.Posts
                .Where(f => EF.Functions.Like(f.Content, '%' + search + '%') && f.PostedOn > skip)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .OrderByDescending(f => f.Likes + f.Comments + f.Reposts)
                .ThenByDescending(f => f.PostedOn)
                .Take(50)
                .ToListAsync(token);

        public Task<List<PostVm>> SearchVideoPosts(string search, DateTime skip, CancellationToken token) =>
            _context.Posts
                .Where(f => f.Video != null && EF.Functions.Like(f.Content, '%' + search + '%') && f.PostedOn > skip)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .OrderByDescending(f => f.Likes + f.Comments + f.Reposts)
                .ThenByDescending(f => f.PostedOn)
                .Take(50)
                .ToListAsync(token);
    }
}
