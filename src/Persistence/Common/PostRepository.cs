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
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly IMapper _mapper;

        public PostRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<int> VerifyPosts(long[] postIds, CancellationToken token) =>
            Query.CountAsync(f => postIds.Contains(f.Id), token);

        public Task<PostVm> FindById(long id, CancellationToken token) =>
            Query.Where(f => f.Id == id)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(token);

        public Task<List<PostVm>> FindPostsFromUsers(IEnumerable<string> userIds, DateTime skip, CancellationToken token) =>
            Query.Where(f => userIds.Any(a => a == f.UserId) && f.CreatedOn > skip)
                .OrderByDescending(f => f.CreatedOn)
                .Take(50)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

        public Task<List<PostVm>> PostReplies(long postId, DateTime skip, CancellationToken token) =>
            Query.Where(f => f.ReplyId == postId)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

        public Task<List<PostVm>> UserPosts(DateTime skip, string username, CancellationToken token) =>
            Query.Include(f => f.User)
                .Where(f => f.CreatedOn > skip && f.User.UserName == username)
                .OrderByDescending(f => f.CreatedOn)
                .Take(50)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

        public Task<List<PostVm>> MultimediaPosts(string username, DateTime skip, CancellationToken token) =>
            Query.Include(f => f.Images)
                .Include(f => f.User)
                .Where(f => (f.Video != null || f.Images.Count > 0) && f.User.UserName == username && f.CreatedOn > skip)
                .OrderByDescending(f => f.CreatedOn)
                .Take(50)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

        public Task<List<PostVm>> SearchImagePosts(string search, DateTime skip, CancellationToken token) =>
            Query.Include(f => f.Images)
                .Where(f => f.Images.Count > 0 && EF.Functions.Like(f.Content, '%' + search + '%') && f.CreatedOn > skip)
                .OrderByDescending(f => f.Likes + f.Comments + f.Reposts)
                .ThenByDescending(f => f.CreatedOn)
                .Take(50)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

        public Task<List<PostVm>> SearchPosts(string search, DateTime skip, CancellationToken token) =>
            Query.Where(f => EF.Functions.Like(f.Content, '%' + search + '%') && f.CreatedOn > skip)
                .OrderByDescending(f => f.Likes + f.Comments + f.Reposts)
                .ThenByDescending(f => f.CreatedOn)
                .Take(50)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

        public Task<List<PostVm>> SearchVideoPosts(string search, DateTime skip, CancellationToken token) =>
            Query.Where(f => f.Video != null && EF.Functions.Like(f.Content, '%' + search + '%') && f.CreatedOn > skip)
                .OrderByDescending(f => f.Likes + f.Comments + f.Reposts)
                .ThenByDescending(f => f.CreatedOn)
                .Take(50)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
    }
}
