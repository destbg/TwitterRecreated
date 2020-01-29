using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common
{
    public class ChatUserRepository : BaseRepository<ChatUser>, IChatUserRepository
    {
        public ChatUserRepository(ITwitterDbContext context) : base(context)
        {
        }

        public Task<ChatUser> FindByUserAndChat(string userId, long chatId, CancellationToken token) =>
            _context.ChatUsers
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ChatId == chatId, token);
    }
}
