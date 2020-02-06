using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Notifications.Command.CreateNotification;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Reposts.Command.CreateRepost
{
    public class CreateRepostHandler : IRequestHandler<CreateRepostCommand>
    {
        private readonly IRepostRepository _repost;
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;
        private readonly IMediator _mediator;

        public CreateRepostHandler(IRepostRepository repost, IPostRepository post, ICurrentUserService currentUser, IMediator mediator)
        {
            _repost = repost ?? throw new ArgumentNullException(nameof(repost));
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Unit> Handle(CreateRepostCommand request, CancellationToken cancellationToken)
        {
            var post = await _post.GetById(request.Id, cancellationToken)
                ?? throw new NotFoundException("Post Id", request.Id);

            if (post.UserId == _currentUser.User.Id)
                throw new BadRequestException("You can't repost your own post");

            var repost = await _repost.FindByUserAndPost(_currentUser.User.Id, request.Id, cancellationToken);

            if (repost == default)
            {
                post.Reposts++;
                await _post.Update(post, cancellationToken);

                await _repost.Create(new Repost
                {
                    Content = request.Content,
                    PostId = request.Id,
                    UserId = _currentUser.User.Id
                }, cancellationToken);

                if (_currentUser.User.Verified)
                {
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        NotificationType = NotificationType.Repost,
                        UserId = post.UserId,
                        PostId = post.Id
                    });
                }
            }
            else
            {
                post.Reposts--;
                await _post.Update(post, cancellationToken);
                await _repost.Delete(repost, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
