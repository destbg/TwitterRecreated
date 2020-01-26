using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Like.Command.Like
{
    public class LikeHandler : IRequestHandler<LikeCommand>
    {
        private readonly IRepository<Post> _post;
        private readonly IRepository<LikedPost> _likedPost;
        private readonly ICurrentUserService _currentUser;

        public LikeHandler(IRepository<Post> post, IRepository<LikedPost> likedPost, ICurrentUserService currentUser)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<Unit> Handle(LikeCommand request, CancellationToken cancellationToken)
        {
            var post = await _post.GetAll()
                .FirstOrDefaultAsync(f => f.Id == request.PostId)
                ?? throw new NotFoundException("Post Id", request.PostId);
            var like = await _likedPost.GetAll()
                .FirstOrDefaultAsync(f => f.PostId == request.PostId
                    && f.UserId == _currentUser.UserId, cancellationToken);
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
