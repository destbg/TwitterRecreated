using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Notifications.Command.CreateNotification;
using Application.Tags.Command.CheckForTags;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Reposts.Command.CreateRepostWithComment
{
    public class CreateRepostWithCommentHandler : IRequestHandler<CreateRepostWithCommentCommand>
    {
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;
        private readonly IMediator _mediator;

        public CreateRepostWithCommentHandler(IPostRepository post, ICurrentUserService currentUser, IMediator mediator)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Unit> Handle(CreateRepostWithCommentCommand request, CancellationToken cancellationToken)
        {
            var post = await _post.GetById(request.RepostId, cancellationToken)
                ?? throw new NotFoundException("Repost Id", request.RepostId);

            if (post.UserId == _currentUser.User.Id)
                throw new BadRequestException("You can't repost your own post");

            post.Reposts++;
            await _post.Update(post, cancellationToken);

            await _post.Create(new Post
            {
                Content = request.Content,
                RepostId = post.Id,
                UserId = _currentUser.User.Id
            }, cancellationToken);

            if (_currentUser.User.Verified)
            {
                await _mediator.Send(new CreateNotificationCommand
                {
                    NotificationType = NotificationType.Repost,
                    PostId = post.Id,
                    UserId = post.UserId
                });
            }

            await _mediator.Send(new CheckForTagsCommand { Content = request.Content });

            return Unit.Value;
        }
    }
}
