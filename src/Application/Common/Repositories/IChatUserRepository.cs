using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IChatUserRepository : IRepository<ChatUser>
    {
        Task<ChatUser> FindByUserAndChat(string userId, long chatId, CancellationToken token);
        Task<AppUser> FindUserInChat(long chatId, string userId, CancellationToken token);
        Task<List<string>> FindUserIdsInChat(string exceptUserId, long chatId, CancellationToken token);
    }
}
