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
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private readonly IMapper _mapper;

        public MessageRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<List<MessageVm>> ChatMessages(long chatId, DateTime skip, CancellationToken token) =>
            _context.Messages
                .Where(f => f.ChatId == chatId && f.CreatedAt > skip)
                .ProjectTo<MessageVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
    }
}
