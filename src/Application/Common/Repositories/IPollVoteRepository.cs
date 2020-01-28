using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IPollVoteRepository : IRepository<PollVote>
    {
        Task<PollVote> GetByUserAndOption(string userId, long optionId, CancellationToken token);
    }
}
