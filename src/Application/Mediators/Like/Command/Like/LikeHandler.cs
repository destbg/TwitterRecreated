using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Common;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Like.Command.Like
{
    public class LikeHandler : IRequestHandler<LikeCommand>
    {
        private readonly IPostRepository _post;
        private readonly ILikedPostRepository _likedPost;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _date;

        public LikeHandler(IPostRepository post, ILikedPostRepository likedPost, ICurrentUserService currentUser, IDateTime date)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        public async Task<Unit> Handle(LikeCommand request, CancellationToken cancellationToken)
        {
            var post = await _post.GetById(request.PostId, cancellationToken)
                ?? throw new NotFoundException("Post Id", request.PostId);
            var like = await _likedPost.FindByUserAndPost(request.PostId, _currentUser.UserId, cancellationToken);
            if (like != null)
            {
                await _likedPost.Delete(like, cancellationToken);
                post.Likes--;
            }
            else
            {
                await _likedPost.Create(new LikedPost
                {
                    PostId = post.Id,
                    UserId = _currentUser.UserId,
                    LikedOn = _date.Now
                }, cancellationToken);
                post.Likes++;
            }
            await _post.Update(post, cancellationToken);
            return Unit.Value;
        }
    }
}
