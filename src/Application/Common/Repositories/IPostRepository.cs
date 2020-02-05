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
        Task<PostVm> FindById(long id, string userId, CancellationToken token);
        Task<List<PostVm>> PostReplies(long postId, string userId, DateTime skip, CancellationToken token);
        Task<List<PostVm>> FindPostsFromUsers(IEnumerable<string> userIds, string userId, DateTime skip, CancellationToken token);
        Task<List<PostVm>> UserPosts(string username, string userId, DateTime skip, CancellationToken token);
        Task<List<PostVm>> MultimediaPosts(string username, string userId, DateTime skip, CancellationToken token);
        Task<List<PostVm>> SearchImagePosts(string search, string userId, DateTime skip, CancellationToken token);
        Task<List<PostVm>> SearchPosts(string search, string userId, DateTime skip, CancellationToken token);
        Task<List<PostVm>> SearchVideoPosts(string search, string userId, DateTime skip, CancellationToken token);
    }
}
