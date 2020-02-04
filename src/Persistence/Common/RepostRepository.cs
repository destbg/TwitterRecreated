using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common
{
    public class RepostRepository : BaseRepository<Repost>, IRepostRepository
    {
        public RepostRepository(ITwitterDbContext context) : base(context)
        {
        }

        public Task<Repost> FindByUserAndPost(string userId, long postId, CancellationToken token) =>
            Query.FirstOrDefaultAsync(f => f.UserId == userId && f.PostId == postId, token);
    }
}
