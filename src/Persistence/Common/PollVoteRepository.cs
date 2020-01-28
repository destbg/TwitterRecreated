using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common
{
    public class PollVoteRepository : BaseRepository<PollVote>, IPollVoteRepository
    {
        public PollVoteRepository(ITwitterDbContext context) : base(context)
        {
        }

        public Task<PollVote> GetByUserAndOption(string userId, long optionId, CancellationToken token) =>
            _context.PollVotes
                .FirstOrDefaultAsync(f => f.UserId == userId && f.OptionId == optionId, token);
    }
}
