using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Replies.Queries.PostReplies
{
    public class PostRepliesHandler : IRequestHandler<PostRepliesQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;
        private readonly ILikedPostRepository _likedPost;
        private readonly ICurrentUserService _currentUser;

        public PostRepliesHandler(IPostRepository post, ILikedPostRepository likedPost, ICurrentUserService currentUser)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<PostVm>> Handle(PostRepliesQuery request, CancellationToken cancellationToken)
        {
            var results = await _post.PostReplies(request.PostId, request.Skip, cancellationToken);

            if (!_currentUser.IsAuthenticated)
                return results;

            var liked = await _likedPost.HasUserLikedPosts(results.Select(f => f.Id), _currentUser.User.Id, cancellationToken);

            return results.Select(f =>
            {
                f.IsLiked = liked.Contains(f.Id);
                return f;
            });
        }
    }
}
