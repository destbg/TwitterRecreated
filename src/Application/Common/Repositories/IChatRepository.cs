using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<List<ChatVm>> UserChats(string userId, CancellationToken token);
        Task<Chat> FindChatByUsers(string[] userIds, bool isGroup, CancellationToken token);
    }
}
