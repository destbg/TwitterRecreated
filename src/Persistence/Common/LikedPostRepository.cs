using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Common
{
    public class LikedPostRepository : GenericRepository<LikedPost>, ILikedPostRepository
    {
        public LikedPostRepository(ITwitterDbContext context) : base(context)
        {
        }

        public async Task<LikedPost> FindByUserAndPost(long postId, string userId, CancellationToken token) =>
            await _context.LikedPosts.FirstOrDefaultAsync(f => f.PostId == postId && f.UserId == userId, token);
    }
}
