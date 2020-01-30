using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using MediatR;

namespace Application.Bookmarks.Command.DeleteBookmark
{
    public class DeleteBookmarkHandler : IRequestHandler<DeleteBookmarkCommand>
    {
        private readonly IBookmarkRepository _bookmark;
        private readonly ICurrentUserService _currentUser;

        public DeleteBookmarkHandler(IBookmarkRepository bookmark, ICurrentUserService currentUser)
        {
            _bookmark = bookmark ?? throw new ArgumentNullException(nameof(bookmark));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<Unit> Handle(DeleteBookmarkCommand request, CancellationToken cancellationToken)
        {
            var bookmark = await _bookmark.GetByIdAndUser(_currentUser.User.Id, request.Id, cancellationToken)
                    ?? throw new NotFoundException("Bookmark Id", request.Id);
            await _bookmark.Delete(bookmark, cancellationToken);
            return Unit.Value;
        }
    }
}
