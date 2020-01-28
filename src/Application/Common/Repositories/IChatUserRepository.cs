using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IChatUserRepository : IRepository<ChatUser>
    {
        Task<ChatUser> FindByUserAndChat(string userId, long chatId, CancellationToken token);
    }
}
