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

        public Task<Chat> FindByUserAndChat(string userId, long chatId, CancellationToken token) =>
            _context.Chats
                .Include(f => f.Users)
                .FirstOrDefaultAsync(f => f.Users.Any(s => s.UserId == userId) && f.Id == chatId, token);

        public Task<List<long>> UserChatIds(string userId, CancellationToken token) =>
            _context.Chats
                .Where(f => f.Users.Any(f => f.UserId == userId))
                .Select(f => f.Id)
                .ToListAsync(token);

        public Task<List<ChatVm>> UserChats(string userId, CancellationToken token) =>
            _context.Chats
                .Include(f => f.Users)
                .Where(f => f.Users.Any(f => f.UserId == userId))
                .Take(20)
                .ProjectTo<ChatVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
    }
}
