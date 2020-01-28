using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Bookmarks.Queries.UserBookmarks
{
    public class UserBookmarksHandler : IRequestHandler<UserBookmarksQuery, IEnumerable<PostVm>>
    {
        private readonly IBookmarkRepository _bookmark;
        private readonly ICurrentUserService _currentUser;

        public UserBookmarksHandler(IBookmarkRepository bookmark, ICurrentUserService currentUser)
        {
            _bookmark = bookmark ?? throw new ArgumentNullException(nameof(bookmark));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<PostVm>> Handle(UserBookmarksQuery request, CancellationToken cancellationToken) =>
            await _bookmark.UserBookmarks(_currentUser.UserId, request.Skip, cancellationToken);
    }
}
