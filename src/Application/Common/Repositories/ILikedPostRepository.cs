using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Repositories
{
    public interface ILikedPostRepository : IRepository<LikedPost>
    {
        Task<LikedPost> FindByUserAndPost(long postId, string userId, CancellationToken token);
    }
}
