using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IHashTagRepository : IRepository<HashTag>
    {
        Task<IEnumerable<HashTagVm>> GetTopTags(string country, CancellationToken token);
    }
}
