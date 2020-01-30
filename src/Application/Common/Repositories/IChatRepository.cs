using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<Chat> FindChatByUsers(string[] userIds, bool isGroup, CancellationToken token);
        Task<Chat> FindByUserAndChat(string userId, long chatId, CancellationToken token);
        Task<List<long>> UserChatIds(string userId, CancellationToken token);
        Task<List<ChatVm>> UserChats(string userId, CancellationToken token);
    }
}
