using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IHashTagRepository : IRepository<HashTag>
    {
        Task<List<HashTagVm>> GetTopTags(string country, CancellationToken token);
        Task<List<HashTagVm>> SearchTags(string search, CancellationToken token);
    }
}
