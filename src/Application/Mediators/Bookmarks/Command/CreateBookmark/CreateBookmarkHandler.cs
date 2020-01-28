using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Posts.Queries.GetPostById;
using Common;
using Domain.Entities;
using MediatR;

namespace Application.Bookmarks.Command.CreateBookmark
{
    public class CreateBookmarkHandler : IRequestHandler<CreateBookmarkCommand>
    {
        private readonly IBookmarkRepository _bookmark;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _date;
        private readonly IMediator _mediator;

        public CreateBookmarkHandler(IBookmarkRepository bookmark, ICurrentUserService currentUser, IDateTime date, IMediator mediator)
        {
            _bookmark = bookmark ?? throw new ArgumentNullException(nameof(bookmark));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _date = date ?? throw new ArgumentNullException(nameof(date));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Unit> Handle(CreateBookmarkCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new GetPostByIdQuery { Id = request.PostId });
            await _bookmark.Create(new Bookmark
            {
                AddedOn = _date.Now,
                PostId = request.PostId,
                UserId = _currentUser.UserId
            }, cancellationToken);
            return Unit.Value;
        }
    }
}
