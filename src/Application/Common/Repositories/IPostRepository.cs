using Application.Common.ViewModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<bool> VerifyPosts(long[] postIds, CancellationToken token);
        Task<IEnumerable<PostVm>> FindPostsFromUsers(DateTime skip, CancellationToken token);
        Task<PostVm> FindById(long id, CancellationToken token);
        Task<IEnumerable<PostVm>> UserPosts(DateTime skip, string username, CancellationToken token);
    }
}
