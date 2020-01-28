using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IBookmarkRepository : IRepository<Bookmark>
    {
        Task<List<PostVm>> UserBookmarks(string userId, DateTime skip, CancellationToken token);
        Task<Bookmark> GetByPostAndUser(string userId, long postId, CancellationToken token);
    }
}
