using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;

namespace Persistence.Common
{
    public class PollOptionRepository : BaseRepository<PollOption>, IPollOptionRepository
    {
        public PollOptionRepository(ITwitterDbContext context) : base(context)
        {
        }
    }
}
