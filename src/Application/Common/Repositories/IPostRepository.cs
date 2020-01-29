using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<int> VerifyPosts(long[] postIds, CancellationToken token);
        Task<PostVm> FindById(long id, CancellationToken token);
        Task<List<PostVm>> PostReplies(long postId, DateTime skip, CancellationToken token);
        Task<List<PostVm>> FindPostsFromUsers(DateTime skip, CancellationToken token);
        Task<List<PostVm>> UserPosts(DateTime skip, string username, CancellationToken token);
        Task<List<PostVm>> MultimediaPosts(string username, DateTime skip, CancellationToken token);
        Task<List<PostVm>> SearchImagePosts(string search, DateTime skip, CancellationToken token);
        Task<List<PostVm>> SearchPosts(string search, DateTime skip, CancellationToken token);
        Task<List<PostVm>> SearchVideoPosts(string search, DateTime skip, CancellationToken token);
    }
}
