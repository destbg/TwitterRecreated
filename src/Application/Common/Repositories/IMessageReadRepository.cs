using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IMessageReadRepository : IRepository<MessageRead>
    {
        Task<List<MessageRead>> FindByUserAndChat(long chatId, string userId, CancellationToken token);
    }
}
