using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common
{
    public class MessageReadRepository : BaseRepository<MessageRead>, IMessageReadRepository
    {
        public MessageReadRepository(ITwitterDbContext context) : base(context)
        {
        }

        public Task<List<MessageRead>> FindByUserAndChat(long chatId, string userId, CancellationToken token) =>
            Query.Include(f => f.Message)
                .Where(f => f.UserId == userId && f.Message.ChatId == chatId)
                .ToListAsync(token);
    }
}
