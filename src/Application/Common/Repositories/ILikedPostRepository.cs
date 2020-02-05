using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface ILikedPostRepository : IRepository<LikedPost>
    {
        Task<LikedPost> FindByUserAndPost(long postId, string userId, CancellationToken token);
        Task<List<PostVm>> UserPosts(string username, string userId, DateTime skip, CancellationToken token);
    }
}
