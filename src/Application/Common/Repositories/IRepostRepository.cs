using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IRepostRepository : IRepository<Repost>
    {
        Task<Repost> FindByUserAndPost(string userId, long postId, CancellationToken token);
    }
}
