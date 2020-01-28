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
    public class BookmarkRepository : BaseRepository<Bookmark>, IBookmarkRepository
    {
        private readonly IMapper _mapper;

        public BookmarkRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<List<PostVm>> UserBookmarks(string userId, DateTime skip, CancellationToken token) =>
            _context.Bookmarks
                .Where(f => f.UserId == userId && f.AddedOn > skip)
                .Select(f => f.Post)
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .Take(50)
                .ToListAsync(token);

        public Task<Bookmark> GetByPostAndUser(string userId, long postId, CancellationToken token) =>
            _context.Bookmarks
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PostId == postId, token);
    }
}
