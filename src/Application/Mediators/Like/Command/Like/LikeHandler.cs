using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using Application.Notifications.Command.CreateNotification;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Like.Command.Like
{
    public class LikeHandler : IRequestHandler<LikeCommand>
    {
        private readonly IPostRepository _post;
        private readonly ILikedPostRepository _likedPost;
        private readonly ICurrentUserService _currentUser;
        private readonly IMainHubService _mainHub;
        private readonly IMediator _mediator;

        public LikeHandler(IPostRepository post, ILikedPostRepository likedPost, ICurrentUserService currentUser, IMainHubService mainHub, IMediator mediator)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Unit> Handle(LikeCommand request, CancellationToken cancellationToken)
        {
            var post = await _post.GetById(request.PostId, cancellationToken)
                ?? throw new NotFoundException("Post Id", request.PostId);
            var like = await _likedPost.FindByUserAndPost(request.PostId, _currentUser.User.Id, cancellationToken);

            post.Likes += like != null ? -1 : 1;

            await _post.Update(post, cancellationToken);

            if (like != null)
            {
                await _likedPost.Delete(like, cancellationToken);

                await _mainHub.SendLikedPost(new LikeVm { IsLike = false, PostId = post.Id });
            }
            else
            {
                await _likedPost.Create(new LikedPost
                {
                    PostId = post.Id,
                    UserId = _currentUser.User.Id
                }, cancellationToken);

                if (_currentUser.User.Verified)
                {
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        NotificationType = NotificationType.Like,
                        PostId = post.Id,
                        UserId = post.UserId
                    });
                }

                await _mainHub.SendLikedPost(new LikeVm { IsLike = true, PostId = post.Id });
            }
            return Unit.Value;
        }
    }
}
