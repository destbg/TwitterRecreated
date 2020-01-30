using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Posts.Queries.GetPostById;
using Domain.Entities;
using MediatR;

namespace Application.Bookmarks.Command.CreateBookmark
{
    public class CreateBookmarkHandler : IRequestHandler<CreateBookmarkCommand>
    {
        private readonly IBookmarkRepository _bookmark;
        private readonly ICurrentUserService _currentUser;
        private readonly IMediator _mediator;

        public CreateBookmarkHandler(IBookmarkRepository bookmark, ICurrentUserService currentUser, IMediator mediator)
        {
            _bookmark = bookmark ?? throw new ArgumentNullException(nameof(bookmark));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Unit> Handle(CreateBookmarkCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new GetPostByIdQuery { Id = request.PostId });
            var bookmark = await _bookmark.GetByPostAndUser(_currentUser.User.Id, request.PostId, cancellationToken);
            if (bookmark != null)
                throw new BadRequestException("This post is already in your bookmarks");
            await _bookmark.Create(new Bookmark
            {
                PostId = request.PostId,
                UserId = _currentUser.User.Id
            }, cancellationToken);
            return Unit.Value;
        }
    }
}
