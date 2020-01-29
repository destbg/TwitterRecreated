using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Like.Command.Like
{
    public class LikeHandler : IRequestHandler<LikeCommand>
    {
        private readonly IPostRepository _post;
        private readonly ILikedPostRepository _likedPost;
        private readonly ICurrentUserService _currentUser;

        public LikeHandler(IPostRepository post, ILikedPostRepository likedPost, ICurrentUserService currentUser)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
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
                    UserId = _currentUser.UserId
                }, cancellationToken);
                post.Likes++;
            }
            await _post.Update(post, cancellationToken);
            return Unit.Value;
        }
    }
}
