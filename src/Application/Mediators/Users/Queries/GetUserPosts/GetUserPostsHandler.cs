using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Users.Queries.GetUserPosts
{
    public class GetUserPostsHandler : IRequestHandler<GetUserPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _posts;
        private readonly ICurrentUserService _currentUser;
        private readonly ILikedPostRepository _likedPost;

        public GetUserPostsHandler(IPostRepository posts, ICurrentUserService currentUser, ILikedPostRepository likedPost)
        {
            _posts = posts ?? throw new ArgumentNullException(nameof(posts));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
        }

        public async Task<IEnumerable<PostVm>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            var results = await _posts.UserPosts(request.Skip, request.Username, cancellationToken);

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
