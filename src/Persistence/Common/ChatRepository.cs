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
using Microsoft.EntityFrameworkCore.Internal;

namespace Persistence.Common
{
    public class ChatRepository : BaseRepository<Chat>, IChatRepository
    {
        private readonly IMapper _mapper;

        public ChatRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Chat> FindChatByUsers(string[] userIds, bool isGroup, CancellationToken token) =>
            _context.Chats
                .Include(f => f.Users)
                .FirstOrDefaultAsync(f => userIds.SequenceEqual(f.Users.Select(s => s.UserId)));

        public Task<List<ChatVm>> UserChats(string userId, CancellationToken token) =>
            _context.Chats
                .Include(f => f.Users)
                .Where(f => f.Users.Any(f => f.UserId == userId))
                .ProjectTo<ChatVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
    }
}
