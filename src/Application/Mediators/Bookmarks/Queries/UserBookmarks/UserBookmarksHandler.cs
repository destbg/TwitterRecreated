using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Bookmarks.Queries.UserBookmarks
{
    public class UserBookmarksHandler : IRequestHandler<UserBookmarksQuery, IEnumerable<BookmarkVm>>
    {
        private readonly IBookmarkRepository _bookmark;
        private readonly ICurrentUserService _currentUser;
        private readonly ILikedPostRepository _likedPost;

        public UserBookmarksHandler(IBookmarkRepository bookmark, ICurrentUserService currentUser, ILikedPostRepository likedPost)
        {
            _bookmark = bookmark ?? throw new ArgumentNullException(nameof(bookmark));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
        }

        public async Task<IEnumerable<BookmarkVm>> Handle(UserBookmarksQuery request, CancellationToken cancellationToken)
        {
            var results = await _bookmark.UserBookmarks(_currentUser.User.Id, request.Skip, cancellationToken);

            var liked = await _likedPost.HasUserLikedPosts(results.Select(f => f.Post.Id), _currentUser.User.Id, cancellationToken);

            return results.Select(f =>
            {
                f.Post.IsLiked = liked.Contains(f.Id);
                return f;
            });
        }
    }
}
