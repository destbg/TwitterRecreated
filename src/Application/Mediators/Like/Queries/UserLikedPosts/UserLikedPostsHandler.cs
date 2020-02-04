using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Like.Queries.UserLikedPosts
{
    public class UserLikedPostsHandler : IRequestHandler<UserLikedPostsQuery, IEnumerable<PostVm>>
    {
        private readonly ILikedPostRepository _likedPost;
        private readonly ICurrentUserService _currentUser;

        public UserLikedPostsHandler(ILikedPostRepository likedPost, ICurrentUserService currentUser)
        {
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<PostVm>> Handle(UserLikedPostsQuery request, CancellationToken cancellationToken)
        {
            var results = await _likedPost.UserPosts(request.Username, request.Skip, cancellationToken);

            var liked = await _likedPost.HasUserLikedPosts(results.Select(f => f.Id), _currentUser.User.Id, cancellationToken);

            return results.Select(f =>
            {
                f.IsLiked = liked.Contains(f.Id);
                return f;
            });
        }
    }
}
