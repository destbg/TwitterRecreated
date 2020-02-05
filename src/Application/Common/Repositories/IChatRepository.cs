using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Chats.Queries.UserChatIds;
using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<Chat> FindChatByUsers(IEnumerable<string> userIds, bool isGroup, CancellationToken token);
        Task<Chat> FindByUserAndChat(string userId, long chatId, CancellationToken token);
        Task<List<long>> UserChatIds(string userId, CancellationToken token);
        Task<List<UserChatCheckResponse>> AllUserChats(string userId, CancellationToken token);
        Task<List<ChatVm>> UserChats(string userId, CancellationToken token);
    }
}
